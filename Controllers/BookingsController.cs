using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TutorBridge.Areas.Identity.Data;
using TutorBridge.Models;
using TutorBridge.ViewModels;
using static TutorBridge.Models.Booking;

namespace TutorBridge.Controllers
{
    public class BookingsController : Controller
    {
        private readonly TutorBridgeContext _context;

        public BookingsController(TutorBridgeContext context)
        {
            _context = context;
        }

        // GET: Bookings
        [Authorize(Roles = "Admin,Tutor")]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                var bookings = await _context.Booking
                    .Include(b => b.User)
                    .Include(b => b.Subject)
                    .Include(b => b.Timeslot)
                    .ThenInclude(t => t.Tutor)
                    .ToListAsync();

                return View(bookings);
            }
            else if (User.IsInRole("Tutor"))
            {
                var bookings = await _context.Booking
                    .Include(b => b.Timeslot)
                    .ThenInclude(t => t.Tutor)
                    .Where(t => t.Timeslot.TutorId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                    .Include(b => b.User)
                    .Include(b => b.Subject)
                    .ToListAsync();

                return View(bookings);
            }
            else
            {
                return Forbid();
            }
        }

        // GET: Bookings/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.User)
                .Include(b => b.Subject)
                .Include(b => b.Timeslot)
                .ThenInclude(t => t.Tutor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        [Authorize]
        public async Task<IActionResult> Book(string id)
        {
            var tutor = await _context.Users.FindAsync(id);

            if (tutor == null)
                return NotFound();

            var availableTimeslots = await _context.Timeslot
                .Where(t => t.TutorId == tutor.Id)
                //.Where(t => t.DateTimeStart > DateTime.Now) Disable for debug TODO remove this.
                .OrderBy(t => t.DateTimeStart)
                .Select(t => new
                {
                    id = t.TimeslotId,
                    start = t.DateTimeStart,
                    end = t.DateTimeEnd,
                    title = $"{t.DateTimeStart:h:mm tt}–{t.DateTimeEnd:h:mm tt}"
                })
                .ToListAsync();

            var availableSubjects = await _context.TutorSubject
                .Where(t => t.TutorId == tutor.Id)
                .Join(_context.Subject, ts => ts.SubjectId, s => s.SubjectId, (ts, s) => new SelectListItem
                {
                    Value = s.SubjectId.ToString(),
                    Text = s.Name
                })
                .ToListAsync();

            ViewBag.Tutor = tutor;
            ViewBag.Timeslots = availableTimeslots;
            ViewBag.Subjects = availableSubjects;

            var booking = new Booking
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            };

            return View(booking);
        }

        [HttpPost, Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book([Bind("TimeslotId,SubjectId")] Booking booking)
        {
            booking.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            booking.Status = BookingStatus.Pending;

            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            return View(booking);
        }

        // GET: Bookings/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Users = await UserDropdown();
            ViewBag.Timeslots = await TimeslotDropdown();
            ViewBag.Subjects = await SubjectDropdown();

            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,TimeslotId,SubjectId,Status")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Users = await UserDropdown();
            ViewBag.Timeslots = await TimeslotDropdown();
            ViewBag.Subjects = await SubjectDropdown();

            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            ViewBag.Users = await UserDropdown();
            ViewBag.Timeslots = await TimeslotDropdown();
            ViewBag.Subjects = await SubjectDropdown();

            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,TimeslotId,SubjectId,Status")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Users = await UserDropdown();
            ViewBag.Timeslots = await TimeslotDropdown();
            ViewBag.Subjects = await SubjectDropdown();

            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.User)
                .Include(b => b.Timeslot)
                .ThenInclude(t => t.Tutor)
                .Include(b => b.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }



            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            if (booking != null)
            {
                _context.Booking.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.Id == id);
        }

        public async Task<IEnumerable<SelectListItem>> UserDropdown()
        {
            return (await _context.Users
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = $"{u.NameFirst} {u.NameLast}"
                })
                .ToListAsync())
                .OrderBy(u => u.Text);
        }

        public async Task<IEnumerable<SelectListItem>> TimeslotDropdown()
        {
            return (await _context.Timeslot
                .Include(t => t.Tutor)
                .Select(t => new SelectListItem
                {
                    Value = t.TimeslotId.ToString(),
                    Text = $"{t.Tutor.NameFirst} {t.Tutor.NameLast} // {t.DateTimeStart:d} {t.DateTimeStart:t} - {t.DateTimeEnd:t}"
                })
                .ToListAsync())
                .OrderBy(t => t.Text);
        }

        public async Task<IEnumerable<SelectListItem>> SubjectDropdown()
        {
            return (await _context.Subject
                .Select(s => new SelectListItem
                {
                    Value = s.SubjectId.ToString(),
                    Text = $"{s.Name}"
                })
                .ToListAsync())
                .OrderBy(s => s.Text);
            ;
        }
    }
}
