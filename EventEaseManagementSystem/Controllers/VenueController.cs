using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using EventEaseManagementSystem.Data;
using EventEaseManagementSystem.Models;

namespace EventEaseManagementSystem.Controllers
{
    public class VenueController : Controller
    {
        private readonly EventEaseDBContext _context;

        private readonly string storageAccountName;
        private readonly string storageAccountKey;
        private readonly string containerName;

        public VenueController(EventEaseDBContext context, IConfiguration config)
        {
            _context = context;
            storageAccountName = config["AzureBlob:storageAccountName"] ?? throw new ArgumentNullException("AzureBlob:storageAccountName");
            storageAccountKey = config["AzureBlob:storageAccountKey"] ?? throw new ArgumentNullException("AzureBlob:storageAccountKey");
            containerName = config["AzureBlob:containerName"] ?? "venue-pictures";
        }

        private BlobContainerClient GetContainerClient()
        {
            var serviceUri = new Uri($"https://{storageAccountName}.blob.core.windows.net");
            var serviceClient = new BlobServiceClient(serviceUri, new StorageSharedKeyCredential(storageAccountName, storageAccountKey));
            return serviceClient.GetBlobContainerClient(containerName);
        }

        
        private async Task<string> UploadImageToBlobAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file provided for upload.");

            var containerClient = GetContainerClient();
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);

            var blobName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var blobClient = containerClient.GetBlobClient(blobName);

            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });

            return blobName;
        }
        


        private string GenerateSasUrl(string blobName)
        {
            if (string.IsNullOrEmpty(blobName))
                return "";

            var blobUri = new Uri($"https://{storageAccountName}.blob.core.windows.net/{containerName}/{blobName}");
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = containerName,
                BlobName = blobName,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(15)
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var sasToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(storageAccountName, storageAccountKey)).ToString();
            return $"{blobUri}?{sasToken}";
        }

        public async Task<IActionResult> Index()
        {
            var venues = await _context.Venues.ToListAsync();
            var venueWithUrls = venues.Select(v => new Venue
            {
                VenueId = v.VenueId,
                VenueName = v.VenueName,
                Location = v.Location,
                Capacity = v.Capacity,
                ImageUrl = string.IsNullOrEmpty(v.ImageUrl) ? "" : GenerateSasUrl(v.ImageUrl)
            }).ToList();

            return View(venueWithUrls);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues
                .FirstOrDefaultAsync(m => m.VenueId == id);

            if (venue == null)
            {
                return NotFound();
            }

            venue.ImageUrl = GenerateSasUrl(venue.ImageUrl);

            return View(venue);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VenueId,VenueName,Location,Capacity,ImageUrl")] Venue venue, IFormFile? VenuePicture)
        {
            if (venue.VenuePicture == null || venue.VenuePicture.Length == 0)
            {
                ModelState.AddModelError("VenuePicture", "Please upload a valid image.");
            }

            if (!ModelState.IsValid)
            {
                return View(venue);
            }

            // Upload image and get URL
            try
            {
                var imageUrl = await UploadImageToBlobAsync(file: VenuePicture);
                venue.ImageUrl = imageUrl;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Image upload failed: {ex.Message}");
                return View(venue);
            }

            _context.Add(venue);
            await _context.SaveChangesAsync();

            // Set temporary message
            TempData["SuccessMessage"] = $"Venue {venue.VenueName} was added successfully!";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var venue = await _context.Venues.FindAsync(id);
            if (venue == null)
                return NotFound();

            return View(venue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VenueId,VenueName,Location,Capacity,ImageUrl")] Venue venue, IFormFile? VenuePicture)
        {
            if (id != venue.VenueId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (VenuePicture != null && VenuePicture.Length > 0)
                    {
                        string blobName = await UploadImageToBlobAsync(VenuePicture);
                        venue.ImageUrl = blobName;
                    }

                    _context.Update(venue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueExists(venue.VenueId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // Set temporary message
                TempData["UpdateMessage"] = $"Venue {venue.VenueName} was updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var venue = await _context.Venues.FirstOrDefaultAsync(v => v.VenueId == id);
            if (venue == null)
                return NotFound();

            return View(venue);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venue = await _context.Venues.FindAsync(id);
            if (venue != null)
            {
                // Delete image blob
                try
                {
                    var containerClient = GetContainerClient();
                    var blobClient = containerClient.GetBlobClient(venue.ImageUrl);
                    await blobClient.DeleteIfExistsAsync();
                }
                catch
                {
                    // Log error if needed
                }

                _context.Venues.Remove(venue);
                await _context.SaveChangesAsync();
            }

            // Set temporary message
            TempData["DeleteMessage"] = $"Venue {venue.VenueName} was deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private bool VenueExists(int id)
        {
            return _context.Venues.Any(v => v.VenueId == id);
        }
    }
}
