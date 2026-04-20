namespace InsuranceAPI.Domain.Entities;

/// <summary>
/// Maps to ACCMOVE table - account movements / receipts.
/// PK: SerNo (identity).
/// </summary>
public class Receipt
{
    public long SerNo { get; set; }
    public string? DocNo { get; set; }
    public string? SubDocNo { get; set; }
    public DateTime? DocDat { get; set; }
    public string? CustNAme { get; set; }
    public double? PayMent { get; set; }
    public double? Amount { get; set; }
    public string? ForW { get; set; }
    public string? AccNo { get; set; }
    public string? Bank { get; set; }
    public string? Note { get; set; }
    public int? EndNo { get; set; }
    public int? LoadNo { get; set; }
    public string? Tp { get; set; }
    public string? Node { get; set; }
    public string? Cur { get; set; }
    public int? PayTp { get; set; }
    public string? UserName { get; set; }
    public string? Branch { get; set; }
    public DateTime Sysdate { get; set; }
    public string? PaymentDetail { get; set; }
    public string? AccountUsed { get; set; }
}
