namespace InsuranceAPI.Domain.Entities;

public class MainJournal
{
    public string DailyNum { get; set; } = string.Empty;
    public DateTime? DailyDate { get; set; }
    public int? DailyType { get; set; }
    public string? AnalysisNum { get; set; }
    public string? Comment { get; set; }
    public int? Currency { get; set; }
    public decimal? Exchange { get; set; }
    public string? CurUser { get; set; }
    public string? RecNo { get; set; }
    public int? DailyCheck { get; set; }
    public string? Branch { get; set; }

    public ICollection<JournalDetail> Details { get; set; } = new List<JournalDetail>();
}

public class JournalDetail
{
    public int Id { get; set; }
    public string DailyNum { get; set; } = string.Empty;
    public int? Type { get; set; }
    public string? AccountNo { get; set; }
    public decimal Dr { get; set; }
    public decimal Cr { get; set; }
    public string? CurUser { get; set; }
    public string? Branch { get; set; }
    public string? Comment { get; set; }
}
