namespace InsuranceAPI.Domain.Entities;

public class Customer
{
    public long Key { get; set; }
    public long CustNo { get; set; }
    public int CustTP { get; set; }
    public string CustName { get; set; } = string.Empty;
    public string CustNameE { get; set; } = string.Empty;
    public string IDNo { get; set; } = string.Empty;
    public string DrCardNo { get; set; } = string.Empty;
    public string? CustNat { get; set; }
    public DateTime RecDate { get; set; }
    public string TelNo { get; set; } = string.Empty;
    public string FaxNo { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int? SpecialCase { get; set; }
    public string? AccNo { get; set; }
    public short? Bank { get; set; }
    public string? BankAcc { get; set; }
    public string? OldCust { get; set; }
    public string? UserName { get; set; }
}
