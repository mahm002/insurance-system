namespace InsuranceAPI.Domain.Entities;

/// <summary>
/// Maps to LogData table.
/// </summary>
public class LogEntry
{
    public long Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Operation { get; set; } = string.Empty;
    public DateTime ActionDate { get; set; }
    public string? IPAddress { get; set; }
}
