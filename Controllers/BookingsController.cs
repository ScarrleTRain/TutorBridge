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
using TutorBridge.Services;
using TutorBridge.ViewModels;
using static TutorBridge.Models.Booking;

namespace TutorBridge.Controllers
{
    public class BookingsController : Controller
    {
        private readonly TutorBridgeContext _context;
        private readonly INotificationService _notificationService;

        public BookingsController(TutorBridgeContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
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

            // Only the student who made the booking, the tutor whose timeslot it belongs to,
            // or an Admin may view its details.
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isOwningStudent = booking.UserId == currentUserId;
            bool isBookingsTutor = booking.Timeslot.TutorId == currentUserId;

            if (!User.IsInRole("Admin") && !isOwningStudent && !isBookingsTutor)
            {
                return Forbid();
            }

            return View(booking);
        }

        [Authorize]
        public async Task<IActionResult> Book(string id)
        {
            var tutor = await _context.Users.FindAsync(id);

            if (tutor == null)
                return NotFound();

            await PopulateBookViewBag(tutor);

            return View(new BookingCreateViewModel());
        }

        [HttpPost, Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(BookingCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var booking = new Booking
                {
                    TimeslotId = model.TimeslotId!.Value,
                    SubjectId = model.SubjectId!.Value,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!,
                    Status = BookingStatus.Pending
                };

                _context.Add(booking);
                await _context.SaveChangesAsync();
                await _notificationService.NotifyBookingCreatedAsync(booking.Id);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            // Re-derive the tutor from the submitted timeslot so the view can render.
            var timeslot = await _context.Timeslot
                .Include(t => t.Tutor)
                .FirstOrDefaultAsync(t => t.TimeslotId == model.TimeslotId);

            if (timeslot == null)
            {
                // No valid timeslot to recover the tutor from — can't redisplay this view sensibly.
                return NotFound();
            }

            await PopulateBookViewBag(timeslot.Tutor);

            return View(model);
        }

        // GET: Bookings/Create
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,TimeslotId,SubjectId,Status")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                await _notificationService.NotifyBookingCreatedAsync(booking.Id);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Users = await UserDropdown();
            ViewBag.Timeslots = await TimeslotDropdown();
            ViewBag.Subjects = await SubjectDropdown();

            return View(booking);
        }

        // GET: Bookings/Edit/5
        [Authorize(Roles = "Admin,Tutor")]
        public async Task<IActionResult> Edit(int? id)
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

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isBookingsTutor = booking.Timeslot.TutorId == currentUserId;

            if (!User.IsInRole("Admin") && !isBookingsTutor)
            {
                return Forbid();
            }

            if (User.IsInRole("Admin"))
            {
                ViewBag.Users = await UserDropdown();
                ViewBag.Timeslots = await TimeslotDropdown();
            }
            else
            {
                ViewBag.Timeslots = await TimeslotDropdown(currentUserId);
            }
            ViewBag.Subjects = await SubjectDropdown(booking.Timeslot.TutorId);

            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin,Tutor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,TimeslotId,SubjectId,Status")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            // Ownership must be checked against the booking's existing timeslot, not the
            // posted one — otherwise a Tutor could point TimeslotId at someone else's slot
            // and have the check pass.
            var existing = await _context.Booking
                .AsNoTracking()
                .Include(b => b.Timeslot)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (existing == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isAdmin = User.IsInRole("Admin");
            bool isBookingsTutor = existing.Timeslot.TutorId == currentUserId;

            if (!isAdmin && !isBookingsTutor)
            {
                return Forbid();
            }

            // The tutor whose timeslot this booking will end up on once saved — used to
            // validate the Subject choice below, and as the redisplay context on failure.
            string targetTutorId;

            if (!isAdmin)
            {
                // Tutors can't reassign a booking to a different student, whatever the form posted.
                booking.UserId = existing.UserId;

                var timeslot = await _context.Timeslot
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.TimeslotId == booking.TimeslotId);

                if (timeslot == null || timeslot.TutorId != currentUserId)
                {
                    ModelState.AddModelError(nameof(booking.TimeslotId), "Select one of your own timeslots.");
                }

                targetTutorId = currentUserId!;
            }
            else
            {
                var timeslot = await _context.Timeslot
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.TimeslotId == booking.TimeslotId);

                if (timeslot == null)
                {
                    ModelState.AddModelError(nameof(booking.TimeslotId), "Select a valid timeslot.");
                    targetTutorId = existing.Timeslot.TutorId;
                }
                else
                {
                    targetTutorId = timeslot.TutorId;
                }
            }

            bool subjectValid = await _context.TutorSubject
                .AnyAsync(ts => ts.TutorId == targetTutorId && ts.SubjectId == booking.SubjectId);

            if (!subjectValid)
            {
                ModelState.AddModelError(nameof(booking.SubjectId), "That tutor doesn't teach the selected subject.");
            }

            if (ModelState.IsValid)
            {
                var previousStatus = existing.Status;

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

                if (previousStatus != BookingStatus.Cancelled && booking.Status == BookingStatus.Cancelled)
                    await _notificationService.NotifyBookingCancelledAsync(booking.Id);
                else
                    await _notificationService.NotifyBookingEditedAsync(booking.Id);

                return RedirectToAction(nameof(Index));
            }

            if (isAdmin)
            {
                ViewBag.Users = await UserDropdown();
                ViewBag.Timeslots = await TimeslotDropdown();
                ViewBag.Subjects = await SubjectDropdown();
            }
            else
            {
                ViewBag.Timeslots = await TimeslotDropdown(currentUserId);
                ViewBag.Subjects = await SubjectDropdown(targetTutorId);
            }

            return View(booking);
        }

        // GET: Bookings/Delete/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            if (booking != null)
            {
                _context.Booking.Remove(booking);
            }

            await _context.SaveChangesAsync();

            if (booking != null)
                await _notificationService.NotifyBookingCancelledAsync(id);

            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.Id == id);
        }

        private async Task PopulateBookViewBag(User tutor)
        {
            ViewBag.Tutor = tutor;

            ViewBag.Timeslots = await _context.Timeslot
                .Where(t => t.TutorId == tutor.Id)
                .OrderBy(t => t.DateTimeStart)
                .Select(t => new
                {
                    id = t.TimeslotId,
                    start = t.DateTimeStart,
                    end = t.DateTimeEnd,
                    title = $"{t.DateTimeStart:h:mm tt}–{t.DateTimeEnd:h:mm tt}"
                })
                .ToListAsync();

            ViewBag.Subjects = await _context.TutorSubject
                .Where(t => t.TutorId == tutor.Id)
                .Join(_context.Subject, ts => ts.SubjectId, s => s.SubjectId, (ts, s) => new SelectListItem
                {
                    Value = s.SubjectId.ToString(),
                    Text = s.Name
                })
                .ToListAsync();
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

        public async Task<IEnumerable<SelectListItem>> TimeslotDropdown(string? tutorId = null)
        {
            var query = _context.Timeslot.Include(t => t.Tutor).AsQueryable();

            if (tutorId != null)
            {
                query = query.Where(t => t.TutorId == tutorId);
            }

            return (await query
                .Select(t => new SelectListItem
                {
                    Value = t.TimeslotId.ToString(),
                    Text = $"{t.Tutor.NameFirst} {t.Tutor.NameLast} // {t.DateTimeStart:d} {t.DateTimeStart:t} - {t.DateTimeEnd:t}"
                })
                .ToListAsync())
                .OrderBy(t => t.Text);
        }

        public async Task<IEnumerable<SelectListItem>> SubjectDropdown(string? tutorId = null)
        {
            if (tutorId == null)
            {
                return (await _context.Subject
                    .Select(s => new SelectListItem
                    {
                        Value = s.SubjectId.ToString(),
                        Text = $"{s.Name}"
                    })
                    .ToListAsync())
                    .OrderBy(s => s.Text);
            }

            return (await _context.TutorSubject
            .Where(ts => ts.TutorId == tutorId)
            .Join(_context.Subject,
                ts => ts.SubjectId,
                s => s.SubjectId,
                (ts, s) => new SelectListItem
                {
                    Value = s.SubjectId.ToString(),
                    Text = s.Name
                })
            .ToListAsync())
            .OrderBy(s => s.Text);
        }
    }
}