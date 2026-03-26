namespace TutorBridge.Models
{
    public class Timeslot
    {
        public int TimeslotId { get; set; }
        public int TutorId { get; set; }
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }
    }
}
