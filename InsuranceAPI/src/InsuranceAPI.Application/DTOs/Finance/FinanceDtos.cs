namespace InsuranceAPI.Application.DTOs.Finance;

// ── Accounts ──
public class AccountDto
{
    public string AccountNo { get; set; } = string.Empty;
    public string? AccountName { get; set; }
    public string? ParentAcc { get; set; }
    public int? Level { get; set; }
    public bool? TransactionAcc { get; set; }
    public string? Branch { get; set; }
    public List<AccountDto> Children { get; set; } = new();
}

public class CreateAccountRequest
{
    public string AccountNo { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string? ParentAcc { get; set; }
    public string? Branch { get; set; }
}

// ── Receipts ──
public class ReceiptDto
{
    public string DocNo { get; set; } = string.Empty;
    public string? SubDocNo { get; set; }
    public DateTime? DocDate { get; set; }
    public string? CustName { get; set; }
    public decimal? Amount { get; set; }
    public string? Type { get; set; }
    public string? Branch { get; set; }
    public string? AccNo { get; set; }
    public string? PayType { get; set; }
    public string? UserName { get; set; }
    public string? Note { get; set; }
}

public class CreateReceiptRequest
{
    public string? CustName { get; set; }
    public decimal Amount { get; set; }
    public string? Type { get; set; }
    public string? Branch { get; set; }
    public string? AccNo { get; set; }
    public string PayType { get; set; } = string.Empty;
    public string? Note { get; set; }
    public string? PolNo { get; set; }
    public int? EndNo { get; set; }
    public int? LoadNo { get; set; }
}

// ── Multi-Payment ──
public class MultiPaymentRequest
{
    public string? CustomerName { get; set; }
    public DateTime MoveDate { get; set; }
    public string? Notes { get; set; }
    public decimal TotalDue { get; set; }
    public string? BranchCode { get; set; }
    public string? PolicyNumber { get; set; }
    public int? EndNo { get; set; }
    public int? LoadNo { get; set; }
    public int? CustNo { get; set; }
    public List<PaymentItem> Payments { get; set; } = new();
}

public class PaymentItem
{
    public int Type { get; set; }
    public string? TypeName { get; set; }
    public decimal Amount { get; set; }
    public string? Details { get; set; }
    public string? Account { get; set; }
}

public class MultiPaymentResponse
{
    public string ReceiptNumber { get; set; } = string.Empty;
    public string DailyNumber { get; set; } = string.Empty;
}

// ── Journal ──
public class JournalEntryDto
{
    public string DailyNum { get; set; } = string.Empty;
    public DateTime? DailyDate { get; set; }
    public string? Comment { get; set; }
    public string? CurUser { get; set; }
    public string? RecNo { get; set; }
    public string? Branch { get; set; }
    public List<JournalDetailDto> Details { get; set; } = new();
}

public class JournalDetailDto
{
    public string? AccountNo { get; set; }
    public string? AccountName { get; set; }
    public decimal Dr { get; set; }
    public decimal Cr { get; set; }
    public string? Comment { get; set; }
}

public class CreateJournalRequest
{
    public string? Comment { get; set; }
    public string? Branch { get; set; }
    public List<JournalLineRequest> Lines { get; set; } = new();
}

public class JournalLineRequest
{
    public string AccountNo { get; set; } = string.Empty;
    public decimal Dr { get; set; }
    public decimal Cr { get; set; }
    public string? Comment { get; set; }
}

// ── Balance Sheet ──
public class BalanceSheetDto
{
    public DateTime AsOfDate { get; set; }
    public string? Branch { get; set; }
    public List<BalanceSheetLineDto> Assets { get; set; } = new();
    public List<BalanceSheetLineDto> Liabilities { get; set; } = new();
    public decimal TotalAssets { get; set; }
    public decimal TotalLiabilities { get; set; }
}

public class BalanceSheetLineDto
{
    public string AccountNo { get; set; } = string.Empty;
    public string? AccountName { get; set; }
    public decimal Balance { get; set; }
}
