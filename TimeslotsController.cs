using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TutorBridge.Areas.Identity.Data;
using TutorBridge.Models;

namespace TutorBridge
{
    public class TimeslotsController : Controller
    {
        private readonly TutorBridgeContext _context;

        public TimeslotsController(TutorBridgeContext context)
        {
            _context = context;
        }

        // GET: Timeslots
        public async Task<IActionResult> Index()
        {
            return View(await _context.Timeslot.ToListAsync());
        }

        // GET: Timeslots/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeslot = await _context.Timeslot
                .FirstOrDefaultAsync(m => m.Id == id);
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
        public async Task<IActionResult> Create([Bind("Id,TutorId,DateTimeStart,DateTimeEnd")] Timeslot timeslot)
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

            var timeslot = await _context.Timeslot.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,TutorId,DateTimeStart,DateTimeEnd")] Timeslot timeslot)
        {
            if (id != timeslot.Id)
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
                    if (!TimeslotExists(timeslot.Id))
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

            var timeslot = await _context.Timeslot
                .FirstOrDefaultAsync(m => m.Id == id);
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
            return _context.Timeslot.Any(e => e.Id == id);
        }
    }
}
