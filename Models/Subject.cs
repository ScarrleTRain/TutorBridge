using System.ComponentModel.DataAnnotations;

namespace TutorBridge.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }

        [Required(ErrorMessage = "Subject name is required")]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Subject description is required")]
        [StringLength(500)]
        public required string Description { get; set; }
    }
}
