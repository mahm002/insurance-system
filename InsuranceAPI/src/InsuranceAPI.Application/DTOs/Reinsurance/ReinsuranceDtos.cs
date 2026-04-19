namespace InsuranceAPI.Application.DTOs.Reinsurance;

public class TreatyDto
{
    public int Id { get; set; }
    public string? TreatyName { get; set; }
    public string? TreatyNo { get; set; }
    public string? SubIns { get; set; }
    public DateTime? EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public decimal? RetentionPercent { get; set; }
    public decimal? RetentionAmount { get; set; }
    public decimal? Capacity { get; set; }
    public string? ReinsurerId { get; set; }
    public string? ReinsurerName { get; set; }
    public decimal? SharePercent { get; set; }
    public string? Branch { get; set; }
    public string? Note { get; set; }
    public bool IsActive { get; set; }
}

public class CreateTreatyRequest
{
    public string TreatyName { get; set; } = string.Empty;
    public string? TreatyNo { get; set; }
    public string SubIns { get; set; } = string.Empty;
    public DateTime EffectiveFrom { get; set; }
    public DateTime EffectiveTo { get; set; }
    public decimal? RetentionPercent { get; set; }
    public decimal? RetentionAmount { get; set; }
    public decimal? Capacity { get; set; }
    public string? ReinsurerId { get; set; }
    public string? ReinsurerName { get; set; }
    public decimal? SharePercent { get; set; }
    public string? Branch { get; set; }
    public string? Note { get; set; }
}

public class UpdateTreatyRequest
{
    public string? TreatyName { get; set; }
    public DateTime? EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public decimal? RetentionPercent { get; set; }
    public decimal? RetentionAmount { get; set; }
    public decimal? Capacity { get; set; }
    public string? ReinsurerName { get; set; }
    public decimal? SharePercent { get; set; }
    public string? Note { get; set; }
    public bool IsActive { get; set; }
}

public class DistributePolicyRequest
{
    public string OrderNo { get; set; } = string.Empty;
    public int EndNo { get; set; }
    public int LoadNo { get; set; }
    public string SubIns { get; set; } = string.Empty;
}

public class DistributeClaimRequest
{
    public string ClmNo { get; set; } = string.Empty;
    public string? PolNo { get; set; }
}

public class RiskProfileDto
{
    public string SubIns { get; set; } = string.Empty;
    public string? SubInsName { get; set; }
    public decimal TotalSumInsured { get; set; }
    public decimal TotalNetPremium { get; set; }
    public decimal TotalGrossPremium { get; set; }
    public int PolicyCount { get; set; }
    public int ClaimCount { get; set; }
    public decimal TotalClaimsPaid { get; set; }
    public decimal LossRatio { get; set; }
}
