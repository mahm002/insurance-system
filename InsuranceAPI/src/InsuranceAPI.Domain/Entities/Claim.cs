namespace InsuranceAPI.Domain.Entities;

public class Claim
{
    public string ClmNo { get; set; } = string.Empty;
    public string? PolNo { get; set; }
    public int? EndNo { get; set; }
    public int LoadNo { get; set; }
    public string SubIns { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;
    public double? SumIns { get; set; }
    public int? GroupNo { get; set; }
    public DateTime ClmSysDate { get; set; }
    public DateTime? ClmDate { get; set; }
    public DateTime? ClmInfDate { get; set; }
    public string? InfName { get; set; }
    public string? PrevName { get; set; }
    public string? ClmPlace { get; set; }
    public string? ClmReason { get; set; }
    public string? DmgDiscription { get; set; }
    public short? Status { get; set; }
    public DateTime? ClmCloseDate { get; set; }
    public string Ret { get; set; } = string.Empty;
    public string? UserPc { get; set; }
    public string? UserName { get; set; }
}
