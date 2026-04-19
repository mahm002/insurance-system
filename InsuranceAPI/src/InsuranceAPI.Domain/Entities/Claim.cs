using InsuranceAPI.Domain.Enums;

namespace InsuranceAPI.Domain.Entities;

public class Claim
{
    public string ClmNo { get; set; } = string.Empty;
    public string? PolNo { get; set; }
    public int? EndNo { get; set; }
    public int? LoadNo { get; set; }
    public string? SubIns { get; set; }
    public string? Branch { get; set; }
    public int? CustNo { get; set; }
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
    public string? CloseUser { get; set; }
    public string? OpenUser { get; set; }
    public DateTime? OpenDate { get; set; }
    public string? Note { get; set; }

    // Navigation
    public Customer? Customer { get; set; }
    public Policy? Policy { get; set; }
}
