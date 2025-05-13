using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEaseManagementSystem.Data; 
using EventEaseManagementSystem.Models;

namespace EventEaseManagementSystem.Controllers
{
    public class BookingViewController : Controller
    {
        private readonly EventEaseDBContext _context;

        public BookingViewController(EventEaseDBContext context)
        {
            _context = context;
        }

        // GET: BookingView
        public async Task<IActionResult> Index(string searchQuery)
        {
            var Bookings = _context.BookingViews.AsQueryable();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                Bookings = Bookings.Where(B =>
                B.BookingId.ToString().Contains(searchQuery) ||
                B.EventName.Contains(searchQuery));
            }

            // Fetch the data from the database
            var bookings = await _context.BookingViews.ToListAsync();
            return View(bookings);
        }

        // GET: BookingView Details
        public async Task<IActionResult> Details(int id)
        {
            var booking = await _context.BookingViews
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }
    }
}

