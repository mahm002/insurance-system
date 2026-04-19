namespace InsuranceAPI.Domain.Entities;

public class ClaimEstimation
{
    public int Id { get; set; }
    public string ClmNo { get; set; } = string.Empty;
    public string? PolNo { get; set; }
    public decimal? Amount { get; set; }
    public string? Description { get; set; }
    public DateTime? EstDate { get; set; }
    public string? EstUser { get; set; }
    public int? EstNo { get; set; }
}
