namespace InsuranceAPI.Application.DTOs.Policies;

public class PolicyDto
{
    public string OrderNo { get; set; } = string.Empty;
    public string? PolNo { get; set; }
    public int EndNo { get; set; }
    public int LoadNo { get; set; }
    public string SubIns { get; set; } = string.Empty;
    public string? SubInsName { get; set; }
    public int CustNo { get; set; }
    public string? CustomerName { get; set; }
    public string? Branch { get; set; }
    public string? BranchName { get; set; }
    public DateTime? CoverFrom { get; set; }
    public DateTime? CoverTo { get; set; }
    public decimal NetPRM { get; set; }
    public decimal TOTPRM { get; set; }
    public decimal? SumInsured { get; set; }
    public decimal? Tax { get; set; }
    public decimal? Stamp { get; set; }
    public decimal? Supervision { get; set; }
    public decimal? IssueFee { get; set; }
    public decimal? Inbox { get; set; }
    public bool? Issued { get; set; }
    public bool? Financed { get; set; }
    public bool? Stopped { get; set; }
    public DateTime? IssuTime { get; set; }
    public string? IssueUser { get; set; }
    public int? SerialNo { get; set; }
    public string? Note { get; set; }
    public int? AgentNo { get; set; }
    public decimal? AgentComm { get; set; }
}

public class CreatePolicyRequest
{
    public string SubIns { get; set; } = string.Empty;
    public int CustNo { get; set; }
    public string? Branch { get; set; }
    public DateTime CoverFrom { get; set; }
    public DateTime CoverTo { get; set; }
    public decimal? SumInsured { get; set; }
    public string? Note { get; set; }
    public int? AgentNo { get; set; }
    public decimal? AgentComm { get; set; }
    public string? CurrencyCode { get; set; }
    public decimal? ExchangeRate { get; set; }
}

public class UpdatePolicyRequest
{
    public int CustNo { get; set; }
    public DateTime CoverFrom { get; set; }
    public DateTime CoverTo { get; set; }
    public decimal? SumInsured { get; set; }
    public string? Note { get; set; }
    public int? AgentNo { get; set; }
    public decimal? AgentComm { get; set; }
}

public class EndorsePolicyRequest
{
    public string OrderNo { get; set; } = string.Empty;
    public string SubIns { get; set; } = string.Empty;
    public DateTime? NewCoverFrom { get; set; }
    public DateTime? NewCoverTo { get; set; }
    public decimal? NewSumInsured { get; set; }
    public string? Note { get; set; }
}

public class IssuePolicyRequest
{
    public string OrderNo { get; set; } = string.Empty;
    public int EndNo { get; set; }
    public int LoadNo { get; set; }
    public string SubIns { get; set; } = string.Empty;
    public int? SerialNo { get; set; }
}

public class CancelPolicyRequest
{
    public string OrderNo { get; set; } = string.Empty;
    public int EndNo { get; set; }
    public int LoadNo { get; set; }
    public string SubIns { get; set; } = string.Empty;
    public string? Reason { get; set; }
}

public class PolicySearchRequest
{
    public string? PolNo { get; set; }
    public string? OrderNo { get; set; }
    public string? SubIns { get; set; }
    public int? CustNo { get; set; }
    public string? CustomerName { get; set; }
    public string? Branch { get; set; }
    public DateTime? CoverFromStart { get; set; }
    public DateTime? CoverFromEnd { get; set; }
    public bool? IsIssued { get; set; }
    public bool? IsFinanced { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class PremiumCalculationResult
{
    public decimal NetPremium { get; set; }
    public decimal Tax { get; set; }
    public decimal Stamp { get; set; }
    public decimal Supervision { get; set; }
    public decimal IssueFee { get; set; }
    public decimal TotalPremium { get; set; }
}
