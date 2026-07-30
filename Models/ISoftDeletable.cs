namespace TutorBridge.Models
{
    public interface ISoftDeletable
    {
        DateTime CreatedAt { get; set; }
        DateTime? DeletedAt { get; set; }
    }
}