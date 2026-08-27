using TutorBridge.Areas.Identity.Data;

namespace TutorBridge.Services;

public interface INotificationService
{
    Task NotifyUserSignedUpAsync(User newUser);
    Task NotifyBookingCreatedAsync(int bookingId);
    Task NotifyBookingCancelledAsync(int bookingId);
    Task NotifyBookingEditedAsync(int bookingId);
}