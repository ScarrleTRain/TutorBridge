using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TutorBridge.Areas.Identity.Data;
using TutorBridge.Models;
using TutorBridge.ViewModels;

namespace TutorBridge.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly TutorBridgeContext _context;

        public HomeController(UserManager<User> userManager, TutorBridgeContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
                return RedirectToAction("Admin");

            var tutors = await _userManager.GetUsersInRoleAsync("Tutor");
            return View(tutors);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Admin()
        {
            var stats = new AdminDashboardStats
            {
                TotalStudents = (await _userManager.GetUsersInRoleAsync("Student")).Count,
                TotalTutors = (await _userManager.GetUsersInRoleAsync("Tutor")).Count,
                PendingBookings = await _context.Booking.CountAsync(b => b.Status == Booking.BookingStatus.Pending),
                ConfirmedBookings = await _context.Booking.CountAsync(b => b.Status == Booking.BookingStatus.Confirmed),
                CancelledBookings = await _context.Booking.CountAsync(b => b.Status == Booking.BookingStatus.Cancelled),
                BookingsThisWeek = await _context.Booking
                    .Join(_context.TimeSlot, b => b.TimeSlotId, t => t.TimeSlotId, (b, t) => t)
                    .CountAsync(t => t.DateTimeStart >= DateTime.Now.AddDays(-7))
            };
            return View(stats);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
