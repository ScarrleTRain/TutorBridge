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

        public byte[]? ProfilePhoto { get; set; }

        public string? ProfilePhotoContentType { get; set; }

        public List<Timeslot> Timeslots { get; set; }



        public Tutor(string nameFirst, string nameLast, string? phone, DateOnly birthDate, string? blurb, byte[]? profilePhoto, string? profilePhotoContentType, List<Timeslot> timeslots)
        {
            NameFirst = nameFirst;
            NameLast = nameLast;
            Phone = phone;
            BirthDate = birthDate;
            Blurb = blurb;
            ProfilePhoto = profilePhoto;
            ProfilePhotoContentType = profilePhotoContentType;
            Timeslots = timeslots;
        }




    }
}
