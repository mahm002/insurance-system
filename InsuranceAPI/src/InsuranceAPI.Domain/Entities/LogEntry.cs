namespace InsuranceAPI.Domain.Entities;

public class LogEntry
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string? Operation { get; set; }
    public string? IPAddress { get; set; }
    public DateTime? LogDate { get; set; }
}
