using System.ComponentModel.DataAnnotations;
using TutorBridge.Areas.Identity.Data;
using TutorBridge.Models;

namespace TutorBridge.ViewModels
{
    public class Tutor
    {
        public string Id { get; set; }

        public string NameFirst { get; set; }

        public string NameLast { get; set; }

        public string? Phone { get; set; }

        public DateOnly BirthDate { get; set; }

        public string? Blurb { get; set; }

        public byte[]? ProfilePhoto { get; set; }

        public string? ProfilePhotoContentType { get; set; }

        public List<Subject> Subjects { get; set; }

        public List<Timeslot> Timeslots { get; set; }

        public Tutor(string id, string nameFirst, string nameLast, string? phone, DateOnly birthDate, string? blurb, byte[]? profilePhoto, string? profilePhotoContentType, List<Subject> subjects, List<Timeslot> timeslots)
        {
            Id = id;
            NameFirst = nameFirst;
            NameLast = nameLast;
            Phone = phone;
            BirthDate = birthDate;
            Blurb = blurb;
            ProfilePhoto = profilePhoto;
            ProfilePhotoContentType = profilePhotoContentType;
            Subjects = subjects;
            Timeslots = timeslots;
        }
    }
}