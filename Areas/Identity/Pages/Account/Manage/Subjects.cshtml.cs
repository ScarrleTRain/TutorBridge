using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TutorBridge.Areas.Identity.Data;
using TutorBridge.Models;

namespace TutorBridge.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Roles = "Tutor")]
    public class SubjectsModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly TutorBridgeContext _context;

        public SubjectsModel(UserManager<User> userManager, TutorBridgeContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public List<SelectListItem> SubjectOptions { get; set; } = new();

        public class InputModel
        {
            public List<int> SelectedSubjectIds { get; set; } = new();
        }

        private async Task LoadAsync(string tutorId)
        {
            var selectedIds = await _context.TutorSubject
                .Where(ts => ts.TutorId == tutorId)
                .Select(ts => ts.SubjectId)
                .ToListAsync();

            var allSubjects = await _context.Subject
                .OrderBy(s => s.Name)
                .ToListAsync();

            SubjectOptions = allSubjects.Select(s => new SelectListItem
            {
                Value = s.SubjectId.ToString(),
                Text = s.Name,
                Selected = selectedIds.Contains(s.SubjectId)
            }).ToList();

            Input = new InputModel { SelectedSubjectIds = selectedIds };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user.Id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var existing = await _context.TutorSubject
                .Where(ts => ts.TutorId == user.Id)
                .ToListAsync();

            var validSubjectIds = await _context.Subject
                .Select(s => s.SubjectId)
                .ToListAsync();

            var submittedIds = (Input.SelectedSubjectIds ?? new List<int>())
                .Where(id => validSubjectIds.Contains(id))
                .ToHashSet();

            if (submittedIds.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "Select at least one subject.");
                await LoadAsync(user.Id);
                return Page();
            }

            var existingIds = existing.Select(ts => ts.SubjectId).ToHashSet();

            var toRemove = existing.Where(ts => !submittedIds.Contains(ts.SubjectId));
            _context.TutorSubject.RemoveRange(toRemove);

            var toAdd = submittedIds
                .Where(id => !existingIds.Contains(id))
                .Select(subjectId => new TutorSubject { TutorId = user.Id, SubjectId = subjectId });
            _context.TutorSubject.AddRange(toAdd);

            await _context.SaveChangesAsync();

            StatusMessage = "Your subjects have been updated";
            return RedirectToPage();
        }
    }
}