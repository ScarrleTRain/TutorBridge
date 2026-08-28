using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TutorBridge.Areas.Identity.Data;

namespace TutorBridge.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly TutorBridgeContext _context;

        public NotificationsController(TutorBridgeContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Unread()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var notifications = await _context.Notification
                .Where(n => n.UserId == currentUserId && n.ReadAt == null)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return PartialView("_NotificationPartial", notifications);
        }

        [HttpPost, Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkRead(int id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var notification = await _context.Notification
                .FirstOrDefaultAsync(n => n.Id == id && n.UserId == currentUserId);

            if (notification == null)
                return NotFound();

            notification.ReadAt ??= DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize]
        public async Task<IActionResult> GoTo(int id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var notification = await _context.Notification
                .FirstOrDefaultAsync(n => n.Id == id && n.UserId == currentUserId);

            if (notification == null)
                return NotFound();

            notification.ReadAt ??= DateTime.UtcNow;
            await _context.SaveChangesAsync();

            if (notification.Link == null)
            {
                var referer = Request.Headers.Referer.ToString();
                return string.IsNullOrEmpty(referer)
                    ? RedirectToAction(nameof(HomeController.Index), "Home")
                    : Redirect(referer);
            }

            return Redirect(notification.Link);
        }
    }
}