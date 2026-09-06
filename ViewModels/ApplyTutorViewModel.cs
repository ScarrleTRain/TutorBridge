using System.ComponentModel.DataAnnotations;

namespace TutorBridge.ViewModels
{
    public class ApplyTutorViewModel
    {
        public bool HasPendingApplication { get; set; }

        public DateTime? PendingSubmittedAt { get; set; }

        [Required(ErrorMessage = "Tell us a bit about yourself")]
        [StringLength(500, ErrorMessage = "Max 500 characters")]
        [Display(Name = "Why would you be a good tutor?")]
        public string? Blurb { get; set; }
    }
}
