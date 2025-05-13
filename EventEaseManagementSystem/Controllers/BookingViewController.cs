using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEaseManagementSystem.Data; // Use your actual DbContext namespace
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
        public async Task<IActionResult> Index()
        {
            var bookings = await _context.BookingViews.ToListAsync();
            return View(bookings);
        }

        // GET: BookingView/Details/5
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

