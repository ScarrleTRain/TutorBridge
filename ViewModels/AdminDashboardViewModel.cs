namespace TutorBridge.ViewModels
{
    public class ChartDataDto
    {
        public List<string> Labels { get; set; } = new();
        public List<int> Values { get; set; } = new();
    }

    public class AdminDashboardViewModel
    {
        //public AdminDashboardStats Stats { get; set; }
        public ChartDataDto UsersByRole { get; set; } = new();
        public ChartDataDto BookingsByStatus { get; set; } = new();
        public ChartDataDto BookingsBySubject { get; set; } = new();
        public ChartDataDto SessionsPerTutor { get; set; } = new();
        public List<BookingsPerWeekDto> BookingsOverTime { get; set; } = [];
        public List<UpcomingSessionDto> UpcomingSessions { get; set; } = [];
        public List<RecentSignupDto> RecentSignups { get; set; } = [];
    }

    public class BookingsPerWeekDto
    {
        public required string WeekLabel { get; set; }
        public int Count { get; set; }
    }

    public class UpcomingSessionDto
    {
        public required string StudentName { get; set; }
        public required string TutorName { get; set; }
        public required string Subject { get; set; }
        public DateTime StartTime { get; set; }
    }

    public class RecentSignupDto
    {
        public required string FullName { get; set; }
        public required string Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}