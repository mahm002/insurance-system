namespace InsuranceAPI.Domain.Entities;

public class Customer
{
    public int CustNo { get; set; }
    public string CustName { get; set; } = string.Empty;
    public string? CustNameE { get; set; }
    public string? TelNo { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public int? SpecialCase { get; set; }
    public string? AccNo { get; set; }
    public string? Branch { get; set; }
    public string? NationalId { get; set; }
    public string? PassportNo { get; set; }
    public DateTime? DateOfBirth { get; set; }
}
