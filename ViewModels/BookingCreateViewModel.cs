using System.ComponentModel.DataAnnotations;

namespace TutorBridge.ViewModels
{
    public class BookingCreateViewModel
    {
        [Required]
        [Display(Name = "Timeslot")]
        public int? TimeslotId { get; set; }

        [Required]
        [Display(Name = "Subject")]
        public int? SubjectId { get; set; }
    }
}
