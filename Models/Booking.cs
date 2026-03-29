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

        [RegularExpression(@"^(Pending|Confirmed|Cancelled)$", ErrorMessage = "Invalid status")]
        public Booking.BookingStatus Status { get; set; }

        [StringLength(30, ErrorMessage = "Max 30 Characters")]
        [RegularExpression(@"^[a-zA-Z\s-]+$", ErrorMessage = "First name can only contain letters")]
        public string? NameFirst { get; set; }

        [StringLength(30, ErrorMessage = "Max 30 Characters")]
        [RegularExpression(@"^[a-zA-Z\s-]+$", ErrorMessage = "Last name can only contain letters")]
        public string? NameLast { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        [RegularExpression(@"^02\d{7,9}$", ErrorMessage = "Enter a valid NZ mobile number")]
        public string? Phone { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }
    }
}
