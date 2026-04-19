namespace InsuranceAPI.Application.DTOs.Claims;

public class ClaimDto
{
    public string ClmNo { get; set; } = string.Empty;
    public string? PolNo { get; set; }
    public int? EndNo { get; set; }
    public int? LoadNo { get; set; }
    public string? SubIns { get; set; }
    public string? Branch { get; set; }
    public int? CustNo { get; set; }
    public string? CustomerName { get; set; }
    public DateTime? LossDate { get; set; }
    public DateTime? NotifyDate { get; set; }
    public string? LossLocation { get; set; }
    public string? LossDescription { get; set; }
    public decimal? EstimatedAmount { get; set; }
    public decimal? SettledAmount { get; set; }
    public decimal? PaidAmount { get; set; }
    public int? GroupNo { get; set; }
    public bool? IsClosed { get; set; }
    public DateTime? CloseDate { get; set; }
    public DateTime? OpenDate { get; set; }
    public string? Note { get; set; }
}

public class OpenClaimRequest
{
    public string PolNo { get; set; } = string.Empty;
    public int EndNo { get; set; }
    public int LoadNo { get; set; }
    public string SubIns { get; set; } = string.Empty;
    public DateTime LossDate { get; set; }
    public DateTime NotifyDate { get; set; }
    public string? LossLocation { get; set; }
    public string LossDescription { get; set; } = string.Empty;
    public decimal? EstimatedAmount { get; set; }
    public int? GroupNo { get; set; }
    public string? Note { get; set; }
}

public class EstimateClaimRequest
{
    public decimal Amount { get; set; }
    public string? Description { get; set; }
}

public class SettleClaimRequest
{
    public decimal Amount { get; set; }
    public string? Description { get; set; }
}

public class CloseClaimRequest
{
    public string? Note { get; set; }
}

public class ClaimSearchRequest
{
    public string? ClmNo { get; set; }
    public string? PolNo { get; set; }
    public string? SubIns { get; set; }
    public int? CustNo { get; set; }
    public string? Branch { get; set; }
    public bool? IsClosed { get; set; }
    public DateTime? LossDateFrom { get; set; }
    public DateTime? LossDateTo { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class ClaimEstimationDto
{
    public int Id { get; set; }
    public string ClmNo { get; set; } = string.Empty;
    public decimal? Amount { get; set; }
    public string? Description { get; set; }
    public DateTime? EstDate { get; set; }
    public string? EstUser { get; set; }
    public int? EstNo { get; set; }
}

public class ClaimSettlementDto
{
    public int Id { get; set; }
    public string ClmNo { get; set; } = string.Empty;
    public int SettlNo { get; set; }
    public decimal? Amount { get; set; }
    public string? Description { get; set; }
    public DateTime? SettlDate { get; set; }
    public string? SettlUser { get; set; }
    public bool? IsPaid { get; set; }
}
