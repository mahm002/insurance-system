namespace InsuranceAPI.Domain.Entities;

public class Account
{
    public string AccountNo { get; set; } = string.Empty;
    public string? AccountName { get; set; }
    public string? ParentAcc { get; set; }
    public string? FullPath { get; set; }
    public int? Level { get; set; }
    public bool? TransactionAcc { get; set; }
    public string? Branch { get; set; }
}
