namespace InsuranceAPI.Domain.Entities;

public class Receipt
{
    public string DocNo { get; set; } = string.Empty;
    public string? SubDocNo { get; set; }
    public DateTime? DocDate { get; set; }
    public string? CustName { get; set; }
    public decimal? Payment { get; set; }
    public decimal? Amount { get; set; }
    public string? ForW { get; set; }
    public int? EndNo { get; set; }
    public int? LoadNo { get; set; }
    public string? Type { get; set; }
    public string? Branch { get; set; }
    public string? AccNo { get; set; }
    public string? Bank { get; set; }
    public string? Currency { get; set; }
    public string? Node { get; set; }
    public string? PayType { get; set; }
    public string? UserName { get; set; }
    public string? Note { get; set; }
    public string? PaymentDetail { get; set; }
    public string? AccountUsed { get; set; }
}
