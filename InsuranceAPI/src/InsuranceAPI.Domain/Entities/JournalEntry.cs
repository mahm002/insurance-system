namespace InsuranceAPI.Domain.Entities;

/// <summary>
/// Maps to MainJournal table - journal header records.
/// PK: (DAILYNUM, DailyTyp, Sn) composite key.
/// </summary>
public class MainJournal
{
    public string DAILYNUM { get; set; } = string.Empty;
    public DateTime? DAILYDTE { get; set; }
    public short DailyTyp { get; set; }
    public string DAILYSRL { get; set; } = string.Empty;
    public string? ANALSNUM { get; set; }
    public bool DailyChk { get; set; }
    public bool DailyPrv { get; set; }
    public string? Comment { get; set; }
    public double PayedValue { get; set; }
    public string? PayedFor { get; set; }
    public short Currency { get; set; }
    public double Exchange { get; set; }
    public string? CurUser { get; set; }
    public string? UpUser { get; set; }
    public string MoveRef { get; set; } = string.Empty;
    public string RecNo { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;
    public string SubBranch { get; set; } = string.Empty;
    public long Sn { get; set; }
}

/// <summary>
/// Maps to Journal table - journal detail/line records.
/// PK: (DAILYNUM, TP, idx) composite key.
/// </summary>
public class JournalDetail
{
    public string DAILYNUM { get; set; } = string.Empty;
    public long? GroupNo { get; set; }
    public int TP { get; set; }
    public string AccountNo { get; set; } = string.Empty;
    public decimal? Dr { get; set; }
    public decimal? Cr { get; set; }
    public string? DocNum { get; set; }
    public string? CustName { get; set; }
    public string? Comment { get; set; }
    public string? Note { get; set; }
    public string? CurUser { get; set; }
    public string Branch { get; set; } = string.Empty;
    public string SubBranch { get; set; } = string.Empty;
    public long Idx { get; set; }
}
