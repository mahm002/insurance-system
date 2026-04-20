namespace InsuranceAPI.Domain.Entities;

public class Branch
{
    public string BranchNo { get; set; } = string.Empty;
    public string BranchCode { get; set; } = string.Empty;
    public bool? Agent { get; set; }
    public string? BranchName { get; set; }
    public string? BranchNameEn { get; set; }
    public string? Address { get; set; }
    public string? Telephone { get; set; }
    public string? FaxNo { get; set; }
    public DateTime? StartAt { get; set; }
    public string? EMail { get; set; }
    public byte[]? Logo { get; set; }
    public string AccountingCode { get; set; } = string.Empty;
    public long ManagerId { get; set; }
    public bool MainCenter { get; set; }
    public bool CompanyOffice { get; set; }
    public string SystemURI { get; set; } = string.Empty;
    public string CashierAccount { get; set; } = string.Empty;
    public string ChequeAccount { get; set; } = string.Empty;
}
