using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TutorBridge.Areas.Identity.Data;

namespace TutorBridge.Models
{
    public class TimeSlot : IValidatableObject
    {
        public int TimeSlotId { get; set; }

        public required string TutorId { get; set; }
        [ForeignKey("TutorId")]
        public User? Tutor { get; set; }
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateTimeEnd <= DateTimeStart)
            {
                yield return new ValidationResult(
                    "End time must be after the start time.",
                    [nameof(DateTimeEnd)]);
            }

            if (DateTimeStart < DateTime.Now)
            {
                yield return new ValidationResult(
                    "Start time cannot be in the past.",
                    [nameof(DateTimeStart)]);
            }

            TimeSpan duration = DateTimeEnd - DateTimeStart;
            if (duration > TimeSpan.FromHours(4))
            {
                yield return new ValidationResult(
                    "A single timeslot can't be longer than 4 hours.",
                    [nameof(DateTimeEnd)]);
            }

            if (duration < TimeSpan.FromMinutes(30))
            {
                yield return new ValidationResult(
                    "A single timeslot can't be shorter than 30 minutes.",
                    [nameof(DateTimeEnd)]);
            }
        }
    }
}
