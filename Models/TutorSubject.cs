using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorBridge.Models
{
    [PrimaryKey(nameof(TutorId), nameof(SubjectId))]
    public class TutorSubject
    {
        public required string TutorId { get; set; }
        public required int SubjectId { get; set; }
    }
}
