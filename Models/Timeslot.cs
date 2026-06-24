using System.ComponentModel.DataAnnotations.Schema;
using TutorBridge.Areas.Identity.Data;

namespace TutorBridge.Models
{
    public class TimeSlot
    {
        public int TimeSlotId { get; set; }

        public required string TutorId { get; set; }
        [ForeignKey("TutorId")]
        public User? Tutor { get; set; }
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }
    }
}
