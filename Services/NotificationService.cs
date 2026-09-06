using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TutorBridge.Areas.Identity.Data;
using TutorBridge.Models;

namespace TutorBridge.Services;

public class NotificationService : INotificationService
{
    private const string AdminRole = "Admin";

    private readonly TutorBridgeContext _context;
    private readonly UserManager<User> _userManager;

    public NotificationService(TutorBridgeContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task NotifyUserSignedUpAsync(User newUser)
    {
        var admins = await _userManager.GetUsersInRoleAsync(AdminRole);

        var notifications = admins.Select(admin => new Notification
        {
            UserId = admin.Id,
            Type = Notification.NotificationType.UserSignedUp,
            Title = "New user registered",
            Message = $"{newUser.Email} just signed up.",
            Link = $"/Users/Details/{newUser.Id}"
        });

        _context.Notification.AddRange(notifications);
        await _context.SaveChangesAsync();
    }

    public async Task NotifyAccountCreatedByAdminAsync(User newUser)
    {
        var admins = await _userManager.GetUsersInRoleAsync(AdminRole);

        var notifications = admins.Select(admin => new Notification
        {
            UserId = admin.Id,
            Type = Notification.NotificationType.UserSignedUp,
            Title = "New user registered",
            Message = $"{newUser.Email} just signed up.",
            Link = $"/Users/Details/{newUser.Id}"
        });

        notifications = notifications.Append(new Notification
        {
            UserId = newUser.Id,
            Type = Notification.NotificationType.UserSignedUp,
            Title = "Set Password",
            Message = $"Set your own password here",
            Link = $"/Identity/Account/Manage/ChangePassword"
        });

        _context.Notification.AddRange(notifications);
        await _context.SaveChangesAsync();
    }

    public Task NotifyBookingCreatedAsync(int bookingId) =>
        CreateForBookingAsync(bookingId, Notification.NotificationType.BookingCreated,
            "Booking created", "Your booking has been created.");

    public Task NotifyBookingCancelledAsync(int bookingId) =>
        CreateForBookingAsync(bookingId, Notification.NotificationType.BookingCancelled,
            "Booking cancelled", "A booking has been cancelled.");

    public Task NotifyBookingEditedAsync(int bookingId) =>
        CreateForBookingAsync(bookingId, Notification.NotificationType.BookingEdited,
            "Booking updated", "A booking has been updated.");

    private async Task CreateForBookingAsync(
        int bookingId, Notification.NotificationType type, string title, string message)
    {
        var booking = await _context.Booking
            .IgnoreQueryFilters()
            .Include(b => b.Timeslot)
                .ThenInclude(t => t.Tutor)
            .FirstOrDefaultAsync(b => b.Id == bookingId);

        if (booking is null)
            return; // shouldn't happen if it was just saved, but guard rather than throw mid-request

        var recipientIds = new[] { booking.UserId, booking.Timeslot.TutorId }.Distinct();

        var notifications = recipientIds.Select(id => new Notification
        {
            UserId = id,
            Type = type,
            Title = title,
            Message = message,
            Link = $"/Bookings/Details/{booking.Id}"
        });

        _context.Notification.AddRange(notifications);
        await _context.SaveChangesAsync();
    }

    public async Task NotifyTutorApplicationSubmittedAsync(int applicationId)
    {
        var application = await _context.TutorApplication
            .IgnoreQueryFilters()
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == applicationId);

        if (application is null)
            return; // shouldn't happen if it was just saved, but guard rather than throw mid-request

        var admins = await _userManager.GetUsersInRoleAsync(AdminRole);

        var notifications = admins.Select(admin => new Notification
        {
            UserId = admin.Id,
            Type = Notification.NotificationType.TutorApplicationSubmitted,
            Title = "New tutor application",
            Message = $"{application.User.Email} applied to become a tutor.",
            Link = $"/TutorApplications/Details/{application.Id}"
        });

        _context.Notification.AddRange(notifications);
        await _context.SaveChangesAsync();
    }

    public async Task NotifyTutorApplicationApprovedAsync(User approvedUser)
    {
        var notification = new Notification
        {
            UserId = approvedUser.Id,
            Type = Notification.NotificationType.TutorApplicationApproved,
            Title = "Application approved",
            Message = "Your tutor application was approved. Add the subjects you'd like to teach to get started.",
            Link = "/Identity/Account/Manage/Subjects"
        };

        _context.Notification.Add(notification);
        await _context.SaveChangesAsync();
    }

    public async Task NotifyTutorApplicationDeniedAsync(User deniedUser, string? reason)
    {
        var notification = new Notification
        {
            UserId = deniedUser.Id,
            Type = Notification.NotificationType.TutorApplicationDenied,
            Title = "Application not approved",
            Message = string.IsNullOrWhiteSpace(reason)
                ? "Your tutor application was not approved."
                : $"Your tutor application was not approved. Reason: {reason}",
            Link = null
        };

        _context.Notification.Add(notification);
        await _context.SaveChangesAsync();
    }
}