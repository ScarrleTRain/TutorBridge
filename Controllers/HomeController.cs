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
            var tutors = await _userManager.GetUsersInRoleAsync("Tutor");
            return View(tutors);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Admin()
        {
            var vm = new AdminDashboardViewModel
            {
                UsersByRole = await GetUsersByRoleAsync(),
                BookingsByStatus = await GetBookingsByStatusAsync(),
                BookingsBySubject = await GetBookingsBySubjectAsync(),
                SessionsPerTutor = await GetSessionsPerTutorAsync(),
                BookingsOverTime = await GetBookingsOverTimeAsync(),
                UpcomingSessions = await GetUpcomingSessionsAsync(),
                RecentSignups = await GetRecentSignupsAsync()
            };

            return View(vm);
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
        
        private async Task<ChartDataDto> GetUsersByRoleAsync()
        {
            var data = await (
                from ur in _context.UserRoles
                join r in _context.Roles on ur.RoleId equals r.Id
                group ur by r.Name into g
                select new { Role = g.Key, Count = g.Count() }
            ).AsNoTracking().ToListAsync();

            return new ChartDataDto
            {
                Labels = data.Select(d => d.Role).ToList(),
                Values = data.Select(d => d.Count).ToList()
            };
        }

        private async Task<ChartDataDto> GetBookingsByStatusAsync()
        {
            var data = await _context.Booking
                .AsNoTracking()
                .GroupBy(b => b.Status)
                .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
                .ToListAsync();

            return new ChartDataDto
            {
                Labels = data.Select(d => d.Status).ToList(),
                Values = data.Select(d => d.Count).ToList()
            };
        }

        private async Task<ChartDataDto> GetBookingsBySubjectAsync()
        {
            var data = await _context.Booking
                .AsNoTracking()
                .Include(b => b.Timeslot)
                .GroupBy(b => b.Subject.Name)
                .Select(g => new { Subject = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .ToListAsync();

            return new ChartDataDto
            {
                Labels = data.Select(d => d.Subject).ToList(),
                Values = data.Select(d => d.Count).ToList()
            };
        }

        private async Task<ChartDataDto> GetSessionsPerTutorAsync()
        {
            var data = await _context.Booking
                .AsNoTracking()
                .Include(b => b.Timeslot).ThenInclude(t => t.Tutor)
                .GroupBy(b => new
                {
                    b.Timeslot.Tutor.Id,
                    b.Timeslot.Tutor.NameFirst,
                    b.Timeslot.Tutor.NameLast
                })
                .Select(g => new
                {
                    g.Key.NameFirst,
                    g.Key.NameLast,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .ToListAsync();

            return new ChartDataDto
            {
                Labels = data.Select(d => $"{d.NameFirst} {d.NameLast}").ToList(),
                Values = data.Select(d => d.Count).ToList()
            };
        }

        private async Task<List<BookingsPerWeekDto>> GetBookingsOverTimeAsync()
        {
            var cutoff = DateTime.UtcNow.AddDays(-56);

            var raw = await _context.Booking
                .AsNoTracking()
                .Where(b => b.CreatedAt >= cutoff)
                .ToListAsync();

            return raw
                .GroupBy(b => System.Globalization.ISOWeek.GetWeekOfYear(b.CreatedAt))
                .OrderBy(g => g.Key)
                .Select(g => new BookingsPerWeekDto
                {
                    WeekLabel = $"Wk {g.Key}",
                    Count = g.Count()
                })
                .ToList();
        }

        private async Task<List<UpcomingSessionDto>> GetUpcomingSessionsAsync()
        {
            return await _context.Booking
                .AsNoTracking()
                .Include(b => b.User)
                .Include(b => b.Timeslot).ThenInclude(t => t.Tutor)
                .Include(b => b.Timeslot)
                .Include(b => b.Subject)
                .Where(b => b.Timeslot.DateTimeStart >= DateTime.UtcNow)
                .OrderBy(b => b.Timeslot.DateTimeStart)
                .Take(5)
                .Select(b => new UpcomingSessionDto
                {
                    StudentName = b.User.FullName,
                    TutorName = b.Timeslot.Tutor.FullName,
                    Subject = b.Subject.Name,
                    StartTime = b.Timeslot.DateTimeStart
                })
                .ToListAsync();
        }

        private async Task<List<RecentSignupDto>> GetRecentSignupsAsync()
        {
            return await _context.Users
                .AsNoTracking()
                .OrderByDescending(u => u.CreatedAt)
                .Take(5)
                .Select(u => new RecentSignupDto
                {
                    FullName = u.FullName,
                    Role = "TBD", // needs the same role join as above, or a computed property
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();
        }
    }
}
