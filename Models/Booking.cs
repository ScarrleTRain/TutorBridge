using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TutorBridge.Areas.Identity.Data;

namespace TutorBridge.Models
{
    public class Booking : ISoftDeletable
    {
        public enum BookingStatus
        {
            Pending,
            Confirmed,
            Cancelled
        }

        public int Id { get; set; }

        [Display(Name = "Student")]
        public required string UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        public int TimeslotId { get; set; }
        [ForeignKey("TimeslotId")]
        public Timeslot Timeslot { get; set; } = null!;

        public int SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public Subject Subject { get; set; } = null!;

        [Required(ErrorMessage = "Booking Status is required")]
        [RegularExpression(@"^(Pending|Confirmed|Cancelled)$", ErrorMessage = "Invalid status")]
        public BookingStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}