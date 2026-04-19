namespace InsuranceAPI.Domain.Entities;

public class Treaty
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
    public bool IsActive { get; set; } = true;
}
