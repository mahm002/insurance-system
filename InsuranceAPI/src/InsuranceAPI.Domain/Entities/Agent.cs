namespace InsuranceAPI.Domain.Entities;

/// <summary>
/// Maps to AgentsCommisions table. Agents are branches with Agent=1 in BranchInfo.
/// This table stores per-SubIns commission rates and account numbers for each agent.
/// </summary>
public class AgentCommission
{
    public long Id { get; set; }
    public string AgentNo { get; set; } = string.Empty;
    public string SubIns { get; set; } = string.Empty;
    public decimal Comm { get; set; }
    public string AccountNo { get; set; } = string.Empty;
}
