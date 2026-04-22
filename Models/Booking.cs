using System.ComponentModel.DataAnnotations;

namespace TutorBridge.Models
{
    public class Booking
    {
        public enum BookingStatus
        {
            Pending,
            Confirmed,
            Cancelled
        }

        public int Id { get; set; }
        public string? UserId { get; set; }
        public int TimeSlotId { get; set; }
        public int SubjectId { get; set; }

        // Makes sure there is a Booking Status, and that it is either Pending, Confirmed, or Cancelled.
        [Required(ErrorMessage = "Booking Status is required")]
        [RegularExpression(@"^(Pending|Confirmed|Cancelled)$", ErrorMessage = "Invalid status")]
        public BookingStatus Status { get; set; }
    }
}
