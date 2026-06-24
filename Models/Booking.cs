using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TutorBridge.Areas.Identity.Data;

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

        [Display(Name = "Student")]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }

        public int TimeSlotId { get; set; }
        [ForeignKey("TimeSlotId")]
        public TimeSlot? TimeSlot { get; set; }

        public int SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public Subject? Subject { get; set; }

        [Required(ErrorMessage = "Booking Status is required")]
        [RegularExpression(@"^(Pending|Confirmed|Cancelled)$", ErrorMessage = "Invalid status")]
        public BookingStatus Status { get; set; }
    }
}