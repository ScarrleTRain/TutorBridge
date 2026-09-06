using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TutorBridge.Areas.Identity.Data;

namespace TutorBridge.Models
{
    public class TutorApplication : ISoftDeletable
    {
        public int Id { get; set; }

        // The student applying to become a tutor.
        public required string UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public User User { get; set; } = null!;

        public TutorApplicationStatus Status { get; set; } = TutorApplicationStatus.Pending;

        // The applicant's pitch, shown to Admin when reviewing. Copied onto
        // User.Blurb if/when the application is approved.
        [StringLength(500, ErrorMessage = "Max 500 characters")]
        public string? Blurb { get; set; }

        public DateTime? ReviewedAt { get; set; }

        // The admin who approved/denied this application, if reviewed.
        public string? ReviewedByUserId { get; set; }
        [ForeignKey("ReviewedByUserId")]
        [ValidateNever]
        public User? Reviewer { get; set; }

        [StringLength(500, ErrorMessage = "Max 500 characters")]
        public string? DenialReason { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public enum TutorApplicationStatus
        {
            Pending,
            Approved,
            Denied
        }
    }
}
