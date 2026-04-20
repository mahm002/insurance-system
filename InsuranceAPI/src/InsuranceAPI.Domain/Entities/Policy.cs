namespace InsuranceAPI.Domain.Entities;

/// <summary>
/// Maps to PolicyFile table. PolicyFile schema was not included in the provided DB script
/// (likely truncated). This entity will be updated once the full PolicyFile CREATE TABLE
/// statement is provided. Current fields are based on VB.NET code analysis.
/// PK: (OrderNo, EndNo, LoadNo, SubIns) composite key.
/// </summary>
public class Policy
{
    public string OrderNo { get; set; } = string.Empty;
    public string? PolNo { get; set; }
    public int EndNo { get; set; }
    public int LoadNo { get; set; }
    public string SubIns { get; set; } = string.Empty;
    public long CustNo { get; set; }
    public string? Branch { get; set; }
    public DateTime? CoverFrom { get; set; }
    public DateTime? CoverTo { get; set; }
    public double NetPRM { get; set; }
    public double TOTPRM { get; set; }
    public double? SumInsured { get; set; }
    public double? Tax { get; set; }
    public double? Stamp { get; set; }
    public double? Supervision { get; set; }
    public double? IssueFee { get; set; }
    public double? Inbox { get; set; }
    public bool? Issued { get; set; }
    public bool? Financed { get; set; }
    public bool? Stopped { get; set; }
    public DateTime? IssuTime { get; set; }
    public string? IssueUser { get; set; }
    public int? SerialNo { get; set; }
    public string? Note { get; set; }
    public int? AgentNo { get; set; }
    public double? AgentComm { get; set; }
    public string? CurrencyCode { get; set; }
    public double? ExchangeRate { get; set; }

    // Navigation
    public Customer? Customer { get; set; }
    public Branch? BranchInfo { get; set; }
}
