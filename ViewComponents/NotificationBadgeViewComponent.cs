using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TutorBridge.Areas.Identity.Data;

namespace TutorBridge.ViewComponents
{
    public class NotificationBadgeViewComponent : ViewComponent
    {
        private readonly TutorBridgeContext _context;

        public NotificationBadgeViewComponent(TutorBridgeContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentUserId = UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

            var unreadCount = await _context.Notification
                .AsNoTracking()
                .CountAsync(n => n.UserId == currentUserId && n.ReadAt == null);

            return View(unreadCount);
        }
    }
}