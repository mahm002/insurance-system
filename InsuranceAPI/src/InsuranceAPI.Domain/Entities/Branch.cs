namespace InsuranceAPI.Domain.Entities;

public class Branch
{
    public string BranchNo { get; set; } = string.Empty;
    public string? BranchName { get; set; }
    public string? BranchNameE { get; set; }
    public string? Address { get; set; }
    public string? TelNo { get; set; }
    public string? FaxNo { get; set; }
    public string? Email { get; set; }
    public int? ManagerId { get; set; }
    public string? CashierAccount { get; set; }
    public string? ChequeAccount { get; set; }
    public int? AccountingCode { get; set; }
    public bool IsActive { get; set; } = true;
}
