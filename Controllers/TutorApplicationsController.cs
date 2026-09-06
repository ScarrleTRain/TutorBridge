using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutorBridge.Areas.Identity.Data;
using TutorBridge.Models;
using TutorBridge.Services;
using TutorBridge.ViewModels;

namespace TutorBridge.Controllers
{
    public class TutorApplicationsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly TutorBridgeContext _context;
        private readonly INotificationService _notificationService;

        public TutorApplicationsController(
            UserManager<User> userManager,
            TutorBridgeContext context,
            INotificationService notificationService)
        {
            _userManager = userManager;
            _context = context;
            _notificationService = notificationService;
        }

        // GET: TutorApplications/Apply
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Apply()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var pending = await _context.TutorApplication
                .Where(a => a.UserId == user.Id && a.Status == TutorApplication.TutorApplicationStatus.Pending)
                .FirstOrDefaultAsync();

            var model = new ApplyTutorViewModel
            {
                HasPendingApplication = pending != null,
                PendingSubmittedAt = pending?.CreatedAt
            };

            return View(model);
        }

        // POST: TutorApplications/Apply
        [HttpPost]
        [Authorize(Roles = "Student")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(ApplyTutorViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Re-check rather than trusting client state, in case of a
            // double submit or a second tab.
            var alreadyPending = await _context.TutorApplication
                .AnyAsync(a => a.UserId == user.Id && a.Status == TutorApplication.TutorApplicationStatus.Pending);

            if (alreadyPending)
            {
                return RedirectToAction(nameof(Apply));
            }

            if (!ModelState.IsValid)
            {
                model.HasPendingApplication = false;
                return View(model);
            }

            var application = new TutorApplication
            {
                UserId = user.Id,
                Blurb = model.Blurb,
                Status = TutorApplication.TutorApplicationStatus.Pending
            };

            _context.TutorApplication.Add(application);
            await _context.SaveChangesAsync();

            await _notificationService.NotifyTutorApplicationSubmittedAsync(application.Id);

            return RedirectToAction(nameof(Apply));
        }

        // GET: TutorApplications
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var applications = await _context.TutorApplication
                .Include(a => a.User)
                .Include(a => a.Reviewer)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            var model = applications.Select(ToViewModel).ToList();

            return View(model);
        }

        // GET: TutorApplications/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.TutorApplication
                .Include(a => a.User)
                .Include(a => a.Reviewer)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (application == null)
            {
                return NotFound();
            }

            return View(ToViewModel(application));
        }

        // POST: TutorApplications/Approve/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var application = await _context.TutorApplication
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (application == null)
            {
                return NotFound();
            }

            if (application.Status != TutorApplication.TutorApplicationStatus.Pending)
            {
                return RedirectToAction(nameof(Details), new { id });
            }

            var applicant = application.User;

            // Copy the pitch onto the live profile immediately.
            applicant.Blurb = application.Blurb;
            await _userManager.UpdateAsync(applicant);

            // Same remove-then-add sequence UsersController.Edit uses, to
            // preserve the single-role-per-user assumption the rest of the
            // app relies on.
            var currentRoles = await _userManager.GetRolesAsync(applicant);
            await _userManager.RemoveFromRolesAsync(applicant, currentRoles);
            await _userManager.AddToRoleAsync(applicant, "Tutor");

            application.Status = TutorApplication.TutorApplicationStatus.Approved;
            application.ReviewedAt = DateTime.UtcNow;
            application.ReviewedByUserId = _userManager.GetUserId(User);

            await _context.SaveChangesAsync();

            await _notificationService.NotifyTutorApplicationApprovedAsync(applicant);

            return RedirectToAction(nameof(Index));
        }

        // POST: TutorApplications/Deny/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deny(int id, string? denialReason)
        {
            var application = await _context.TutorApplication
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (application == null)
            {
                return NotFound();
            }

            if (application.Status != TutorApplication.TutorApplicationStatus.Pending)
            {
                return RedirectToAction(nameof(Details), new { id });
            }

            application.Status = TutorApplication.TutorApplicationStatus.Denied;
            application.DenialReason = denialReason;
            application.ReviewedAt = DateTime.UtcNow;
            application.ReviewedByUserId = _userManager.GetUserId(User);

            await _context.SaveChangesAsync();

            await _notificationService.NotifyTutorApplicationDeniedAsync(application.User, denialReason);

            return RedirectToAction(nameof(Index));
        }

        private static TutorApplicationViewModel ToViewModel(TutorApplication application)
        {
            return new TutorApplicationViewModel
            {
                Id = application.Id,
                ApplicantId = application.UserId,
                ApplicantName = $"{application.User.NameFirst} {application.User.NameLast}",
                ApplicantEmail = application.User.Email ?? "",
                Status = application.Status.ToString(),
                Blurb = application.Blurb,
                SubmittedAt = application.CreatedAt,
                ReviewedAt = application.ReviewedAt,
                ReviewerName = application.Reviewer == null
                    ? null
                    : $"{application.Reviewer.NameFirst} {application.Reviewer.NameLast}",
                DenialReason = application.DenialReason
            };
        }
    }
}
