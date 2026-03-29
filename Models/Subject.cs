using System.ComponentModel.DataAnnotations;

namespace TutorBridge.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }

        [Required(ErrorMessage = "Subject name is required")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "Subject description is required")]
        public required string Description { get; set; }
    }
}
