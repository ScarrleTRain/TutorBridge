using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
        [ValidateNever]
        public User Tutor { get; set; } = null!;
        public required int SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        [ValidateNever]
        public Subject Subject { get; set; } = null!;
    }
}
