namespace TutorBridge.Models
{
    public class Booking
    {
        public enum BookingStatus
        {
            Pending,
            Confirmed,
            Cancelled
        }

        public int Id { get; set; }
        public int? UserId { get; set; }
        public int TimeSlotId { get; set; }
        public int SubjectId { get; set; }
        public BookingStatus Status { get; set; }
        public string? NameFirst { get; set; }
        public string? NameLast { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
