using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using TutorBridge.Areas.Identity.Data;

namespace TutorBridge.Models
{
    public class Notification : ISoftDeletable
    {
        public int Id { get; set; }

        public required string UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public User User { get; set; } = null!;

        public NotificationType Type { get; set; }

        public required string Title { get; set; }

        public required string Message { get; set; }

        public string? Link { get; set; }

        public DateTime? ReadAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public enum NotificationType
        {
            UserSignedUp,
            BookingCreated,
            BookingCancelled,
            BookingEdited
        }
    }
}