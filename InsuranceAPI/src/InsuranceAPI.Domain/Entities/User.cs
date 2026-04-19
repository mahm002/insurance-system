using InsuranceAPI.Domain.Enums;

namespace InsuranceAPI.Domain.Entities;

public class User
{
    public int AccountNo { get; set; }
    public string AccountLogIn { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string? Password { get; set; }
    public int AccountPermSys { get; set; }
    public int AccountPermClm { get; set; }
    public int AccountPermFin { get; set; }
    public int AccountPermRe { get; set; }
    public int AccountPermMan { get; set; }
    public int AccountSysManag { get; set; }
    public string? Branch { get; set; }
    public decimal AccLimit { get; set; }
    public bool IsActive { get; set; } = true;
}
