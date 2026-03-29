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
        public Booking.BookingStatus Status { get; set; }

        [StringLength(30, ErrorMessage = "Max 30 Characters")]
        public string? NameFirst { get; set; }
        [StringLength(30, ErrorMessage = "Max 30 Characters")]
        public string? NameLast { get; set; }
        [Phone(ErrorMessage = "Invalid phone number")]
        public string? Phone { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }
    }
}
