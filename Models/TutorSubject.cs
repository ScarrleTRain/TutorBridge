using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TutorBridge.Areas.Identity.Data;

namespace TutorBridge.Models
{
    [PrimaryKey(nameof(TutorId), nameof(SubjectId))]
    public class TutorSubject
    {
        public required string TutorId { get; set; }
        [ForeignKey("TutorId")]
        public User? Tutor { get; set; }
        public required int SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public Subject? Subject { get; set; }
    }
}
