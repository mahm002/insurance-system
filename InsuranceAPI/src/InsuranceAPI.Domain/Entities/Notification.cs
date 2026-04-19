namespace InsuranceAPI.Domain.Entities;

public class Notification
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Message { get; set; }
    public string? Type { get; set; }
    public string? TargetUser { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? RelatedEntity { get; set; }
    public string? RelatedId { get; set; }
}
