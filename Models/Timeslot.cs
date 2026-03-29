namespace TutorBridge.Models
{
    public class Timeslot
    {
        public int TimeSlotId { get; set; }

        public required string TutorId { get; set; }
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }
    }
}
