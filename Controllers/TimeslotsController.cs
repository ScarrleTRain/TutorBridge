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
    public class TimeslotsController : Controller
    {
        private readonly TutorBridgeContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public TimeslotsController(TutorBridgeContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        // GET: Timeslots
        [Authorize(Roles = "Admin,Tutor")]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                var timeslots = await _context.TimeSlot.Include(t => t.Tutor).ToListAsync();

                var bookedIds = await _context.Booking
                    .Select(b => b.TimeSlotId)
                    .ToHashSetAsync();

                ViewBag.BookedIds = bookedIds;

                return View(timeslots);
            }
            else if (User.IsInRole("Tutor"))
            {
                var timeslots = await _context.TimeSlot.Where(t => t.TutorId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                                                       .Include(t => t.Tutor)
                                                       .ToListAsync();

                var bookedIds = await _context.Booking
                    .Select(b => b.TimeSlotId)
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

            var timeslot = await _context.TimeSlot
                .FirstOrDefaultAsync(m => m.TimeSlotId == id);
            if (timeslot == null)
            {
                return NotFound();
            }

            return View(timeslot);
        }

        // GET: Timeslots/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Timeslots/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TimeSlotId,TutorId,DateTimeStart,DateTimeEnd")] TimeSlot timeslot)
        {
            if (ModelState.IsValid)
            {
                _context.Add(timeslot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(timeslot);
        }

        // GET: Timeslots/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeslot = await _context.TimeSlot.FindAsync(id);
            if (timeslot == null)
            {
                return NotFound();
            }
            return View(timeslot);
        }

        // POST: Timeslots/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TimeSlotId,TutorId,DateTimeStart,DateTimeEnd")] TimeSlot timeslot)
        {
            if (id != timeslot.TimeSlotId)
            {
                return NotFound();
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
                    if (!TimeslotExists(timeslot.TimeSlotId))
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
            return View(timeslot);
        }

        // GET: Timeslots/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeslot = await _context.TimeSlot
                .FirstOrDefaultAsync(m => m.TimeSlotId == id);
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
            var timeslot = await _context.TimeSlot.FindAsync(id);
            if (timeslot != null)
            {
                _context.TimeSlot.Remove(timeslot);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TimeslotExists(int id)
        {
            return _context.TimeSlot.Any(e => e.TimeSlotId == id);
        }
    }
}
