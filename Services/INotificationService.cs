using TutorBridge.Areas.Identity.Data;

namespace TutorBridge.Services;

public interface INotificationService
{
    Task NotifyUserSignedUpAsync(User newUser);
    Task NotifyAccountCreatedByAdminAsync(User newUser);
    Task NotifyBookingCreatedAsync(int bookingId);
    Task NotifyBookingCancelledAsync(int bookingId);
    Task NotifyBookingEditedAsync(int bookingId);
    Task NotifyTutorApplicationSubmittedAsync(int applicationId);
    Task NotifyTutorApplicationApprovedAsync(User approvedUser);
    Task NotifyTutorApplicationDeniedAsync(User deniedUser, string? reason);
}