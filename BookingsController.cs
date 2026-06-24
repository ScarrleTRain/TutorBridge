using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TutorBridge.Areas.Identity.Data;
using TutorBridge.Controllers;
using TutorBridge.Models;
using TutorBridge.ViewModels;
using static TutorBridge.Models.Booking;

namespace TutorBridge
{
    public class BookingsController : Controller
    {
        private readonly TutorBridgeContext _context;

        public BookingsController(TutorBridgeContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            return View(await _context.Booking.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
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

            var availableTimeSlots = await _context.Timeslot
                .Where(t => t.TutorId == tutor.Id)
                //.Where(t => t.DateTimeStart > DateTime.Now) Disable for debug TODO remove this.
                .OrderBy(t => t.DateTimeStart)
                .Select(t => new
                {
                    id = t.TimeSlotId,
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
            ViewBag.TimeSlots = availableTimeSlots;
            ViewBag.Subjects = availableSubjects;

            var booking = new Booking
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            };

            return View(booking);
        }

        [HttpPost, Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book([Bind("TimeSlotId,SubjectId")] Booking booking)
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,TimeSlotId,SubjectId,Status")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,TimeSlotId,SubjectId,Status")] Booking booking)
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
    }
}
