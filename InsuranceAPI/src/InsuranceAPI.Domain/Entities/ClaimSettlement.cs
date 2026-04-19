namespace InsuranceAPI.Domain.Entities;

public class ClaimSettlement
{
    public int Id { get; set; }
    public string ClmNo { get; set; } = string.Empty;
    public string? PolNo { get; set; }
    public int SettlNo { get; set; }
    public decimal? Amount { get; set; }
    public string? Description { get; set; }
    public DateTime? SettlDate { get; set; }
    public string? SettlUser { get; set; }
    public string? Branch { get; set; }
    public bool? IsPaid { get; set; }
}
