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

namespace TutorBridge.Controllers
{
    [Authorize(Roles = "Admin,Tutor")]
    public class TimeslotsController : Controller
    {
        private readonly TutorBridgeContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public TimeslotsController(TutorBridgeContext context, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: Timeslots
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                var timeslots = await _context.Timeslot.Include(t => t.Tutor).ToListAsync();

                var bookedIds = await _context.Booking
                    .Select(b => b.TimeslotId)
                    .ToHashSetAsync();

                ViewBag.BookedIds = bookedIds;

                return View(timeslots);
            }
            else if (User.IsInRole("Tutor"))
            {
                var timeslots = await _context.Timeslot.Where(t => t.TutorId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                                                       .Include(t => t.Tutor)
                                                       .ToListAsync();

                var bookedIds = await _context.Booking
                    .Select(b => b.TimeslotId)
                    .ToHashSetAsync();

                ViewBag.BookedIds = bookedIds;

                return View(timeslots);
            }
            else
            {
                return Forbid(); 
            }
        }

        // GET: Timeslots/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeslot = await _context.Timeslot
                .Include(t => t.Tutor)
                .FirstOrDefaultAsync(m => m.TimeslotId == id);
            if (timeslot == null)
            {
                return NotFound();
            }

            return View(timeslot);
        }

        // GET: Timeslots/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Tutors = await TutorDropdown();

            return View();
        }

        // POST: Timeslots/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TimeslotId,TutorId,DateTimeStart,DateTimeEnd")] Timeslot timeslot)
        {
            if (ModelState.IsValid)
            {
                bool overlaps = await _context.Timeslot.AnyAsync(t =>
                    t.TutorId == timeslot.TutorId &&
                    t.DateTimeStart < timeslot.DateTimeEnd &&
                    t.DateTimeEnd > timeslot.DateTimeStart);

                if (overlaps)
                {
                    ModelState.AddModelError(nameof(timeslot.DateTimeStart),
                        "This tutor already has a timeslot that overlaps with this time.");
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(timeslot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Tutors = await TutorDropdown();

            return View(timeslot);
        }

        // GET: Timeslots/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeslot = await _context.Timeslot.FindAsync(id);
            if (timeslot == null)
            {
                return NotFound();
            }

            ViewBag.Tutors = await TutorDropdown();

            return View(timeslot);
        }

        // POST: Timeslots/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TimeslotId,TutorId,DateTimeStart,DateTimeEnd")] Timeslot timeslot)
        {
            if (id != timeslot.TimeslotId)
            {
                return NotFound();
            }

            var existing = await _context.Timeslot.FindAsync(id);
            if (existing == null) return NotFound();

            if (existing.IsPast())
            {
                ModelState.AddModelError("", "This timeslot has already started and can no longer be edited.");
                return View("Edit", timeslot);
            }
            
            if (timeslot.Bookings.Any(b => b.Status != Booking.BookingStatus.Cancelled))
            {
                ModelState.AddModelError("", "This timeslot has an active booking. Please cancel it first.");
                return View("Delete", timeslot); // explicit view name since action is "DeleteConfirmed"
            }

            if (ModelState.IsValid)
            {
                bool overlaps = await _context.Timeslot.AnyAsync(t =>
                    t.TutorId == timeslot.TutorId &&
                    t.DateTimeStart < timeslot.DateTimeEnd &&
                    t.DateTimeEnd > timeslot.DateTimeStart);

                if (overlaps)
                {
                    ModelState.AddModelError(nameof(timeslot.DateTimeStart),
                        "This tutor already has a timeslot that overlaps with this time.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timeslot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimeslotExists(timeslot.TimeslotId))
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

            ViewBag.Tutors = await TutorDropdown();

            return View(timeslot);
        }

        // GET: Timeslots/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeslot = await _context.Timeslot
                .Include(t => t.Tutor)
                .FirstOrDefaultAsync(m => m.TimeslotId == id);
            if (timeslot == null)
            {
                return NotFound();
            }

            return View(timeslot);
        }

        // POST: Timeslots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var timeslot = await _context.Timeslot.FindAsync(id);
            if (timeslot != null)
            {
                _context.Timeslot.Remove(timeslot);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TimeslotExists(int id)
        {
            return _context.Timeslot.Any(e => e.TimeslotId == id);
        }

        public async Task<IEnumerable<SelectListItem>> TutorDropdown()
        {
             return (await _userManager.GetUsersInRoleAsync("Tutor"))
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = $"{u.NameFirst} {u.NameLast}"
                })
                .ToList().OrderBy(u => u.Text);
        }
    }
}
