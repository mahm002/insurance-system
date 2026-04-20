namespace InsuranceAPI.Domain.Entities;

/// <summary>
/// Maps to MainSattelement table.
/// PK: (ClmNo, TPID, No) composite key.
/// </summary>
public class ClaimSettlement
{
    public string ClmNo { get; set; } = string.Empty;
    public short TPID { get; set; }
    public int No { get; set; }
    public string PayTo { get; set; } = string.Empty;
    public string SettelementDesc { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string? DAILYNUM { get; set; }
    public DateTime? DAILYDTE { get; set; }
    public string? UserName { get; set; }
    public long SerNo { get; set; }
}
