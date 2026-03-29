namespace TutorBridge.Models
{
    public class TutorSubject
    {
        public int TutorSubjectId { get; set; }

        public required string TutorId { get; set; }
        public int SubjectId { get; set; }
    }
}
