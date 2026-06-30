using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using TutorBridge.Areas.Identity.Data;
using TutorBridge.Models;

namespace TutorBridge.ViewModels
{
    public class AdminDashboardStats
    {
        [DisplayName("Total Students")]
        public int TotalStudents { get; set; }

        [DisplayName("Total Tutors")]
        public int TotalTutors { get; set; }

        [DisplayName("Pending Bookings")]
        public int PendingBookings { get; set; }

        [DisplayName("Confirmed Bookings")]
        public int ConfirmedBookings { get; set; }

        [DisplayName("Cancelled Bookings")]
        public int CancelledBookings { get; set; }

        [DisplayName("Bookings This Week")]
        public int BookingsThisWeek { get; set; }
    }
}
