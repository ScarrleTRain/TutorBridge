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

        // Makes sure there is a First name, and that it is less than 30 chars, and only letters, spaces or hyphens.
        [StringLength(30, ErrorMessage = "Max 30 Characters")]
        [RegularExpression(@"^[a-zA-Z\s-]+$", ErrorMessage = "First name can only contain letters")]
        public string? NameFirst { get; set; }

        // Makes sure there is a last name, and that it is less than 30 chars, and only letters, spaces or hyphens.
        [StringLength(30, ErrorMessage = "Max 30 Characters")]
        [RegularExpression(@"^[a-zA-Z\s-]+$", ErrorMessage = "Last name can only contain letters")]
        public string? NameLast { get; set; }

        // Makes sure that it begins with 02, and then 7 to 9 digits, and is a valid phone number.
        [Phone(ErrorMessage = "Invalid phone number")]
        [RegularExpression(@"^02\d{7,9}$", ErrorMessage = "Enter a valid NZ mobile number")]
        public string? Phone { get; set; }

        // Makes sure that it is a valid email address.
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }
    }
}
