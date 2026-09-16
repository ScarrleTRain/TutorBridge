using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
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
            // for redisplay on fail
            var timeslotForRedisplay = await _context.Timeslot
                .Include(t => t.Tutor)
                .FirstOrDefaultAsync(t => t.TimeslotId == model.TimeslotId);

            if (ModelState.IsValid)
            {
                using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

                if (await ValidateBookingRelationshipsAsync(model.TimeslotId!.Value, model.SubjectId!.Value))
                {
                    var booking = new Booking
                    {
                        TimeslotId = model.TimeslotId!.Value,
                        SubjectId = model.SubjectId!.Value,
                        UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!,
                        Status = BookingStatus.Pending
                    };

                    try
                    {
                        _context.Add(booking);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        await _notificationService.NotifyBookingCreatedAsync(booking.Id);
                        TempData["StatusMessage"] = "Your booking request has been sent. " +
                            "You'll be notified once the tutor responds.";
                        return RedirectToAction(nameof(Details), new { id = booking.Id });
                    }
                    catch (DbUpdateException ex) when (IsActiveTimeslotUniqueViolation(ex))
                    {
                        // server backup
                        ModelState.AddModelError(nameof(model.TimeslotId), "That timeslot is already booked.");
                    }
                }

                await transaction.RollbackAsync();
            }

            if (timeslotForRedisplay == null)
            {
                // No valid timeslot to recover the tutor from — can't redisplay this view sensibly.
                return NotFound();
            }

            await PopulateBookViewBag(timeslotForRedisplay.Tutor);

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
                using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

                if (await ValidateBookingRelationshipsAsync(booking.TimeslotId, booking.SubjectId))
                {
                    try
                    {
                        _context.Add(booking);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        await _notificationService.NotifyBookingCreatedAsync(booking.Id);
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateException ex) when (IsActiveTimeslotUniqueViolation(ex))
                    {
                        ModelState.AddModelError(nameof(booking.TimeslotId), "That timeslot is already booked.");
                    }
                }

                await transaction.RollbackAsync();
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
                ViewBag.Timeslots = await BookableTimeslotDropdown(booking.TimeslotId);
            }
            else
            {
                ViewBag.Timeslots = await BookableTimeslotDropdown(booking.TimeslotId, currentUserId);
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

            // Check ownership against previous record
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

            // Force no user change
            if (!isAdmin)
            {
                booking.UserId = existing.UserId;
            }

            // 
            var timeslotForContext = await _context.Timeslot
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TimeslotId == booking.TimeslotId);

            // If admin, TutorId from timeslot, else own ID
            string targetTutorId = isAdmin
                ? (timeslotForContext?.TutorId ?? existing.Timeslot.TutorId)
                : currentUserId!;

            bool timeslotUnchanged = booking.TimeslotId == existing.TimeslotId;

            if (ModelState.IsValid)
            {
                // isolated changes
                using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

                // validate
                bool relationshipsValid = await ValidateBookingRelationshipsAsync(
                    booking.TimeslotId,
                    booking.SubjectId,
                    excludingBookingId: booking.Id,
                    skipFutureCheck: timeslotUnchanged,
                    requiredTutorId: isAdmin ? null : currentUserId);

                if (relationshipsValid)
                {
                    var previousStatus = existing.Status;

                    try
                    {
                        _context.Update(booking);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    // 
                    catch (DbUpdateConcurrencyException)
                    {
                        await transaction.RollbackAsync();
                        if (!BookingExists(booking.Id))
                        {
                            return NotFound();
                        }
                        throw;
                    }
                    catch (DbUpdateException ex) when (IsActiveTimeslotUniqueViolation(ex))
                    {
                        ModelState.AddModelError(nameof(booking.TimeslotId), "That timeslot is already booked.");
                        await transaction.RollbackAsync();
                        relationshipsValid = false;
                    }

                    // Notify
                    if (relationshipsValid)
                    {
                        if (previousStatus != BookingStatus.Cancelled && booking.Status == BookingStatus.Cancelled)
                            await _notificationService.NotifyBookingCancelledAsync(booking.Id);
                        else
                            await _notificationService.NotifyBookingEditedAsync(booking.Id);

                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    await transaction.RollbackAsync();
                }
            }

            if (isAdmin)
            {
                ViewBag.Users = await UserDropdown();
                ViewBag.Timeslots = await BookableTimeslotDropdown(booking.TimeslotId);
                ViewBag.Subjects = await SubjectDropdown();
            }
            else
            {
                ViewBag.Timeslots = await BookableTimeslotDropdown(booking.TimeslotId, currentUserId);
                ViewBag.Subjects = await SubjectDropdown(targetTutorId);
            }

            return View(booking);
        }

        // POST: Bookings/Confirm/5
        [HttpPost]
        [Authorize(Roles = "Tutor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(int id)
        {
            var booking = await _context.Booking
                .Include(b => b.Timeslot)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (booking.Timeslot.TutorId != currentUserId)
            {
                return Forbid();
            }

            // Ignore stale/duplicate clicks rather than erroring — only a Pending booking can be confirmed.
            if (booking.Status == BookingStatus.Pending)
            {
                booking.Status = BookingStatus.Confirmed;
                await _context.SaveChangesAsync();
                await _notificationService.NotifyBookingConfirmedAsync(booking.Id);
                TempData["StatusMessage"] = "Booking confirmed.";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Bookings/Deny/5
        [HttpPost]
        [Authorize(Roles = "Tutor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deny(int id)
        {
            var booking = await _context.Booking
                .Include(b => b.Timeslot)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (booking.Timeslot.TutorId != currentUserId)
            {
                return Forbid();
            }

            // Ignore stale/duplicate clicks rather than erroring — only a Pending booking can be denied.
            if (booking.Status == BookingStatus.Pending)
            {
                booking.Status = BookingStatus.Cancelled;
                await _context.SaveChangesAsync();
                await _notificationService.NotifyBookingCancelledAsync(booking.Id);
                TempData["StatusMessage"] = "Booking denied.";
            }

            return RedirectToAction(nameof(Index));
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

        /// <summary>
        /// Re-validates a booking's relationships against the current database state: the
        /// timeslot exists, is in the future (unless skipFutureCheck), isn't already actively
        /// booked by a different booking, and its tutor actually teaches the chosen subject.
        /// When requiredTutorId is given (non-admin Book/Edit), also enforces the timeslot
        /// belongs to that tutor. Adds ModelState errors for any failures.
        ///
        /// Must be called from inside a Serializable transaction that goes on to perform the
        /// insert/update — the SELECT here is what takes the range locks that make "no two
        /// requests can book the same timeslot" actually hold under concurrency. Called outside
        /// a transaction, or at a lower isolation level, it's just a point-in-time check with
        /// the same race window the audit finding described.
        /// </summary>
        private async Task<bool> ValidateBookingRelationshipsAsync(
            int timeslotId,
            int subjectId,
            int? excludingBookingId = null,
            bool skipFutureCheck = false,
            string? requiredTutorId = null)
        {
            var timeslot = await _context.Timeslot
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TimeslotId == timeslotId);

            if (timeslot == null)
            {
                ModelState.AddModelError(nameof(Booking.TimeslotId), "Select a valid timeslot.");
                return false;
            }

            bool isValid = true;

            // Check if it belongs to them / Are admin
            if (requiredTutorId != null && timeslot.TutorId != requiredTutorId)
            {
                ModelState.AddModelError(nameof(Booking.TimeslotId), "Select one of your own timeslots.");
                isValid = false;
            }

            // Past-lock
            if (!skipFutureCheck && timeslot.DateTimeStart <= DateTime.Now)
            {
                ModelState.AddModelError(nameof(Booking.TimeslotId), "That timeslot has already passed.");
                isValid = false;
            }

            // Check if it has any non cancelled bookings
            bool timeslotTaken = await _context.Booking
                .AnyAsync(b => b.TimeslotId == timeslotId
                    && b.Status != BookingStatus.Cancelled
                    && (excludingBookingId == null || b.Id != excludingBookingId.Value));

            if (timeslotTaken)
            {
                ModelState.AddModelError(nameof(Booking.TimeslotId), "That timeslot is already booked.");
                isValid = false;
            }

            // Check if tutor actually teaches subject
            bool subjectValid = await _context.TutorSubject
                .AnyAsync(ts => ts.TutorId == timeslot.TutorId && ts.SubjectId == subjectId);

            if (!subjectValid)
            {
                ModelState.AddModelError(nameof(Booking.SubjectId), "That tutor doesn't teach the selected subject.");
                isValid = false;
            }

            return isValid;
        }

        // true of the exception was made by the IX_Booking_TimeslotId_ActiveUnique constraint
        private static bool IsActiveTimeslotUniqueViolation(DbUpdateException ex)
        {
            return ex.InnerException is SqlException sqlEx
                && (sqlEx.Number == 2601 || sqlEx.Number == 2627)
                && sqlEx.Message.Contains("IX_Booking_TimeslotId_ActiveUnique");
        }

        private async Task PopulateBookViewBag(User tutor)
        {
            ViewBag.Tutor = tutor;

            // Timeslots that already have a non-cancelled booking must not be offered again.
            var bookedIds = await _context.Booking
                .Where(b => b.Status != BookingStatus.Cancelled)
                .Select(b => b.TimeslotId)
                .ToHashSetAsync();

            ViewBag.Timeslots = await _context.Timeslot
                .Where(t => t.TutorId == tutor.Id)
                .Where(t => t.DateTimeStart > DateTime.Now)
                .Where(t => !bookedIds.Contains(t.TimeslotId))
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

        /// <summary>
        /// Same shape as <see cref="TimeslotDropdown"/>, but for editing an existing booking:
        /// excludes past and already-(actively-)booked timeslots, while always keeping
        /// currentTimeslotId itself selectable so the booking's existing choice never
        /// disappears from its own edit form.
        /// </summary>
        public async Task<IEnumerable<SelectListItem>> BookableTimeslotDropdown(int currentTimeslotId, string? tutorId = null)
        {
            var bookedIds = await _context.Booking
                .Where(b => b.TimeslotId != currentTimeslotId && b.Status != BookingStatus.Cancelled)
                .Select(b => b.TimeslotId)
                .ToHashSetAsync();

            var query = _context.Timeslot.Include(t => t.Tutor).AsQueryable();

            if (tutorId != null)
            {
                query = query.Where(t => t.TutorId == tutorId);
            }

            query = query.Where(t =>
                t.TimeslotId == currentTimeslotId ||
                (t.DateTimeStart > DateTime.Now && !bookedIds.Contains(t.TimeslotId)));

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