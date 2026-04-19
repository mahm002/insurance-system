namespace InsuranceAPI.Domain.Entities;

public class Agent
{
    public int AgentNo { get; set; }
    public string AgentName { get; set; } = string.Empty;
    public string? AgentNameE { get; set; }
    public string? Address { get; set; }
    public string? TelNo { get; set; }
    public string? Email { get; set; }
    public string? Branch { get; set; }
    public decimal? CommissionRate { get; set; }
    public string? AccNo { get; set; }
    public bool IsActive { get; set; } = true;
}
