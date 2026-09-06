namespace TutorBridge.ViewModels
{
    public class TutorApplicationViewModel
    {
        public int Id { get; set; }
        public string ApplicantId { get; set; } = string.Empty;
        public string ApplicantName { get; set; } = string.Empty;
        public string ApplicantEmail { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Blurb { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public string? ReviewerName { get; set; }
        public string? DenialReason { get; set; }
    }
}
