namespace InsuranceAPI.Domain.Entities;

/// <summary>
/// Maps to Estimation table.
/// PK: (TPID, ClmNo, Date) composite key.
/// </summary>
public class ClaimEstimation
{
    public int Sn { get; set; }
    public short TPID { get; set; }
    public string ClmNo { get; set; } = string.Empty;
    public string? PolNo { get; set; }
    public double? Value { get; set; }
    public DateTime Date { get; set; }
    public DateTime? SysDate { get; set; }
}
