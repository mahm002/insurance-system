namespace InsuranceAPI.Domain.Entities;

public class User
{
    public int AccountNo { get; set; }
    public string AccountLogIn { get; set; } = string.Empty;
    public string? AccountName { get; set; }
    public string AccountPassWord { get; set; } = string.Empty;
    public string AccountPermSys { get; set; } = string.Empty;
    public string AccountPermClm { get; set; } = string.Empty;
    public string AccountPermFin { get; set; } = string.Empty;
    public string AccountPermRe { get; set; } = string.Empty;
    public string AccountPermMan { get; set; } = string.Empty;
    public string? AccountSysManag { get; set; }
    public double? AccLimit { get; set; }
    public string Branch { get; set; } = string.Empty;
    public string AddedBy { get; set; } = string.Empty;
    public string ModifiedBy { get; set; } = string.Empty;
    public DateTime ModifyDate { get; set; }
    public byte[]? Signature { get; set; }
    public int? Dept { get; set; }
    public bool Stop { get; set; }
}
