namespace InsuranceAPI.Domain.Entities;

public class SubSystem
{
    public string SubSysNo { get; set; } = string.Empty;
    public string? SubSysName { get; set; }
    public string? MainSys { get; set; }
    public string? Branch { get; set; }
    public string? EditForm { get; set; }
    public string? ExtraInfo { get; set; }
    public string? GroupFile { get; set; }
    public string? EndFile { get; set; }
    public decimal? IssuVal { get; set; }
    public decimal? StmVal { get; set; }
    public decimal? WakalaVal { get; set; }
    public bool? IsGrouped { get; set; }
    public bool? IsReins { get; set; }
}
