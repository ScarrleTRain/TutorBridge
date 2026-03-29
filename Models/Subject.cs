using System.ComponentModel.DataAnnotations;

namespace TutorBridge.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }

        // Makes sure there is a subject name, and that it is less than 100 chars
        [Required(ErrorMessage = "Subject name is required")] 
        [StringLength(100, ErrorMessage = "Max 100 Characters")]
        public required string Name { get; set; }

        // Makes sure there is a subject description, and that it is less than 500 chars
        [Required(ErrorMessage = "Subject description is required")]
        [StringLength(500, ErrorMessage = "Max 500 Characters")]
        public required string Description { get; set; }
    }
}
