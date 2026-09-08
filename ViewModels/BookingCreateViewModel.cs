using System.ComponentModel.DataAnnotations;

namespace TutorBridge.ViewModels
{
    public class BookingCreateViewModel
    {
        [Required]
        public int? TimeslotId { get; set; }

        [Required]
        public int? SubjectId { get; set; }
    }
}
