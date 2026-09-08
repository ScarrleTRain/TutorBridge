using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TutorBridge.Areas.Identity.Data;
using TutorBridge.Models;
using TutorBridge.ViewModels;

namespace TutorBridge.Controllers
{
    public class TutorsController : Controller
    {
        private readonly TutorBridgeContext _context;
        private readonly UserManager<User> _userManager;

        public TutorsController(TutorBridgeContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Tutors
        public async Task<IActionResult> Index(int? subjectId)
        {
            var tutorUsers = await _userManager.GetUsersInRoleAsync("Tutor");
            var tutorIds = tutorUsers.Select(u => u.Id).ToList();

            var tutorSubjects = await _context.TutorSubject
                .Where(ts => tutorIds.Contains(ts.TutorId))
                .Include(ts => ts.Subject)
                .ToListAsync();

            if (subjectId.HasValue)
            {
                var matchingIds = tutorSubjects
                    .Where(ts => ts.SubjectId == subjectId.Value)
                    .Select(ts => ts.TutorId)
                    .ToHashSet();

                tutorUsers = tutorUsers.Where(u => matchingIds.Contains(u.Id)).ToList();
            }

            var tutors = tutorUsers
                .Select(u => new Tutor(
                    u.Id,
                    u.NameFirst,
                    u.NameLast,
                    u.Phone,
                    u.BirthDate,
                    u.Blurb,
                    u.ProfilePhoto,
                    u.ProfilePhotoContentType,
                    tutorSubjects.Where(ts => ts.TutorId == u.Id).Select(ts => ts.Subject).ToList(),
                    new List<Timeslot>()))
                .OrderBy(t => t.NameFirst)
                .ToList();

            ViewBag.Subjects = await SubjectDropdown();
            ViewBag.SelectedSubjectId = subjectId;

            return View(tutors);
        }

        // GET: Tutors/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
                return NotFound();

            var tutorUser = await _userManager.FindByIdAsync(id);
            if (tutorUser == null || !await _userManager.IsInRoleAsync(tutorUser, "Tutor"))
                return NotFound();

            var subjects = await _context.TutorSubject
                .Where(ts => ts.TutorId == id)
                .Include(ts => ts.Subject)
                .Select(ts => ts.Subject)
                .ToListAsync();

            var openTimeslots = (await _context.Timeslot
                    .Where(t => t.TutorId == id)
                    .Include(t => t.Bookings)
                    .ToListAsync())
                .Where(t => t.CanBeModified())
                .OrderBy(t => t.DateTimeStart)
                .ToList();

            var tutor = new Tutor(
                tutorUser.Id,
                tutorUser.NameFirst,
                tutorUser.NameLast,
                tutorUser.Phone,
                tutorUser.BirthDate,
                tutorUser.Blurb,
                tutorUser.ProfilePhoto,
                tutorUser.ProfilePhotoContentType,
                subjects,
                openTimeslots);

            return View(tutor);
        }

        private async Task<IEnumerable<SelectListItem>> SubjectDropdown()
        {
            return (await _context.Subject
                    .Select(s => new SelectListItem
                    {
                        Value = s.SubjectId.ToString(),
                        Text = s.Name
                    })
                    .ToListAsync())
                .OrderBy(s => s.Text);
        }
    }
}