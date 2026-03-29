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
    public class TutorSubjectsController : Controller
    {
        private readonly TutorBridgeContext _context;

        public TutorSubjectsController(TutorBridgeContext context)
        {
            _context = context;
        }

        // GET: TutorSubjects
        public async Task<IActionResult> Index()
        {
            return View(await _context.TutorSubject.ToListAsync());
        }

        // GET: TutorSubjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutorSubject = await _context.TutorSubject
                .FirstOrDefaultAsync(m => m.TutorSubjectId == id);
            if (tutorSubject == null)
            {
                return NotFound();
            }

            return View(tutorSubject);
        }

        // GET: TutorSubjects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TutorSubjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TutorSubjectId,TutorId,SubjectId")] TutorSubject tutorSubject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tutorSubject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tutorSubject);
        }

        // GET: TutorSubjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutorSubject = await _context.TutorSubject.FindAsync(id);
            if (tutorSubject == null)
            {
                return NotFound();
            }
            return View(tutorSubject);
        }

        // POST: TutorSubjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TutorSubjectId,TutorId,SubjectId")] TutorSubject tutorSubject)
        {
            if (id != tutorSubject.TutorSubjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tutorSubject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TutorSubjectExists(tutorSubject.TutorSubjectId))
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
            return View(tutorSubject);
        }

        // GET: TutorSubjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutorSubject = await _context.TutorSubject
                .FirstOrDefaultAsync(m => m.TutorSubjectId == id);
            if (tutorSubject == null)
            {
                return NotFound();
            }

            return View(tutorSubject);
        }

        // POST: TutorSubjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tutorSubject = await _context.TutorSubject.FindAsync(id);
            if (tutorSubject != null)
            {
                _context.TutorSubject.Remove(tutorSubject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TutorSubjectExists(int id)
        {
            return _context.TutorSubject.Any(e => e.TutorSubjectId == id);
        }
    }
}
