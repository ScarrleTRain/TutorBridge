using System.ComponentModel.DataAnnotations;
using TutorBridge.Areas.Identity.Data;
using TutorBridge.Models;

namespace TutorBridge.ViewModels
{
    public class Tutor
    {
        public string NameFirst { get; set; }

        public string NameLast { get; set; }

        public string? Phone { get; set; }

        public DateOnly BirthDate { get; set; }

        public string? Blurb { get; set; }

        public string? ProfilePhoto { get; set; }

        public List<Timeslot> Timeslots { get; set; }

        public Tutor(string nameFirst, string nameLast, string? phone, DateOnly birthDate, string? blurb, string? profilePhoto, List<Timeslot> timeslots)
        {
            NameFirst = nameFirst;
            NameLast = nameLast;
            Phone = phone;
            BirthDate = birthDate;
            Blurb = blurb;
            ProfilePhoto = profilePhoto;
            Timeslots = timeslots;
        }

        public Tutor(User tutor, List<Timeslot> timeslots)
        {
            NameFirst = tutor.NameFirst;
            NameLast = tutor.NameLast;
            Phone = tutor.Phone;
            BirthDate = tutor.BirthDate;
            Blurb = tutor.Blurb;
            ProfilePhoto = tutor.ProfilePhoto;
            Timeslots = timeslots;
        }
    }
}
