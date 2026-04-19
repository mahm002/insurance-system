using InsuranceAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InsuranceAPI.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Agent> Agents => Set<Agent>();
    public DbSet<Policy> Policies => Set<Policy>();
    public DbSet<Claim> Claims => Set<Claim>();
    public DbSet<ClaimEstimation> ClaimEstimations => Set<ClaimEstimation>();
    public DbSet<ClaimSettlement> ClaimSettlements => Set<ClaimSettlement>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<MainJournal> Journals => Set<MainJournal>();
    public DbSet<JournalDetail> JournalDetails => Set<JournalDetail>();
    public DbSet<Receipt> Receipts => Set<Receipt>();
    public DbSet<Treaty> Treaties => Set<Treaty>();
    public DbSet<SubSystem> SubSystems => Set<SubSystem>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<LogEntry> LogEntries => Set<LogEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User → AccountFile table
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("AccountFile");
            e.HasKey(u => u.AccountNo);
            e.Property(u => u.AccountLogIn).HasMaxLength(100);
            e.Property(u => u.AccountName).HasMaxLength(200);
            e.Property(u => u.Password).HasMaxLength(500);
            e.Property(u => u.Branch).HasMaxLength(50);
            e.Property(u => u.AccLimit).HasColumnType("decimal(18,2)");
        });

        // Branch → BranchInfo table
        modelBuilder.Entity<Branch>(e =>
        {
            e.ToTable("BranchInfo");
            e.HasKey(b => b.BranchNo);
            e.Property(b => b.BranchNo).HasMaxLength(50);
            e.Property(b => b.BranchName).HasMaxLength(200);
            e.Property(b => b.BranchNameE).HasMaxLength(200);
            e.Property(b => b.Address).HasMaxLength(500);
            e.Property(b => b.TelNo).HasMaxLength(50);
            e.Property(b => b.FaxNo).HasMaxLength(50);
            e.Property(b => b.Email).HasMaxLength(200);
            e.Property(b => b.CashierAccount).HasMaxLength(50);
            e.Property(b => b.ChequeAccount).HasMaxLength(50);
        });

        // Customer → CustomerFile table
        modelBuilder.Entity<Customer>(e =>
        {
            e.ToTable("CustomerFile");
            e.HasKey(c => c.CustNo);
            e.Property(c => c.CustName).HasMaxLength(200);
            e.Property(c => c.CustNameE).HasMaxLength(200);
            e.Property(c => c.TelNo).HasMaxLength(50);
            e.Property(c => c.Address).HasMaxLength(500);
            e.Property(c => c.Email).HasMaxLength(200);
            e.Property(c => c.AccNo).HasMaxLength(50);
            e.Property(c => c.Branch).HasMaxLength(50);
            e.Property(c => c.NationalId).HasMaxLength(50);
            e.Property(c => c.PassportNo).HasMaxLength(50);
        });

        // Agent → AgentFile table
        modelBuilder.Entity<Agent>(e =>
        {
            e.ToTable("AgentFile");
            e.HasKey(a => a.AgentNo);
            e.Property(a => a.AgentName).HasMaxLength(200);
            e.Property(a => a.AgentNameE).HasMaxLength(200);
            e.Property(a => a.Address).HasMaxLength(500);
            e.Property(a => a.TelNo).HasMaxLength(50);
            e.Property(a => a.Email).HasMaxLength(200);
            e.Property(a => a.Branch).HasMaxLength(50);
            e.Property(a => a.CommissionRate).HasColumnType("decimal(18,4)");
            e.Property(a => a.AccNo).HasMaxLength(50);
        });

        // Policy → PolicyFile table
        modelBuilder.Entity<Policy>(e =>
        {
            e.ToTable("PolicyFile");
            e.HasKey(p => new { p.OrderNo, p.EndNo, p.LoadNo, p.SubIns });
            e.Property(p => p.OrderNo).HasMaxLength(50);
            e.Property(p => p.PolNo).HasMaxLength(50);
            e.Property(p => p.SubIns).HasMaxLength(20);
            e.Property(p => p.Branch).HasMaxLength(50);
            e.Property(p => p.NetPRM).HasColumnType("decimal(18,2)");
            e.Property(p => p.TOTPRM).HasColumnType("decimal(18,2)");
            e.Property(p => p.SumInsured).HasColumnType("decimal(18,2)");
            e.Property(p => p.Tax).HasColumnType("decimal(18,2)");
            e.Property(p => p.Stamp).HasColumnType("decimal(18,2)");
            e.Property(p => p.Supervision).HasColumnType("decimal(18,2)");
            e.Property(p => p.IssueFee).HasColumnType("decimal(18,2)");
            e.Property(p => p.Inbox).HasColumnType("decimal(18,2)");
            e.Property(p => p.IssueUser).HasMaxLength(100);
            e.Property(p => p.Note).HasMaxLength(1000);
            e.Property(p => p.AgentComm).HasColumnType("decimal(18,4)");
            e.Property(p => p.CurrencyCode).HasMaxLength(10);
            e.Property(p => p.ExchangeRate).HasColumnType("decimal(18,6)");
            e.HasOne(p => p.Customer).WithMany().HasForeignKey(p => p.CustNo).OnDelete(DeleteBehavior.NoAction);
            e.HasOne(p => p.BranchInfo).WithMany().HasForeignKey(p => p.Branch).OnDelete(DeleteBehavior.NoAction);
        });

        // Claim → MainClaimFile table
        modelBuilder.Entity<Claim>(e =>
        {
            e.ToTable("MainClaimFile");
            e.HasKey(c => c.ClmNo);
            e.Property(c => c.ClmNo).HasMaxLength(50);
            e.Property(c => c.PolNo).HasMaxLength(50);
            e.Property(c => c.SubIns).HasMaxLength(20);
            e.Property(c => c.Branch).HasMaxLength(50);
            e.Property(c => c.LossLocation).HasMaxLength(500);
            e.Property(c => c.LossDescription).HasMaxLength(2000);
            e.Property(c => c.EstimatedAmount).HasColumnType("decimal(18,2)");
            e.Property(c => c.SettledAmount).HasColumnType("decimal(18,2)");
            e.Property(c => c.PaidAmount).HasColumnType("decimal(18,2)");
            e.Property(c => c.CloseUser).HasMaxLength(100);
            e.Property(c => c.OpenUser).HasMaxLength(100);
            e.Property(c => c.Note).HasMaxLength(2000);
        });

        // ClaimEstimation → ClaimEstimation table
        modelBuilder.Entity<ClaimEstimation>(e =>
        {
            e.ToTable("ClaimEstimation");
            e.HasKey(ce => ce.Id);
            e.Property(ce => ce.ClmNo).HasMaxLength(50);
            e.Property(ce => ce.PolNo).HasMaxLength(50);
            e.Property(ce => ce.Amount).HasColumnType("decimal(18,2)");
            e.Property(ce => ce.Description).HasMaxLength(1000);
            e.Property(ce => ce.EstUser).HasMaxLength(100);
        });

        // ClaimSettlement → ClaimSettlement table
        modelBuilder.Entity<ClaimSettlement>(e =>
        {
            e.ToTable("ClaimSettlement");
            e.HasKey(cs => cs.Id);
            e.Property(cs => cs.ClmNo).HasMaxLength(50);
            e.Property(cs => cs.PolNo).HasMaxLength(50);
            e.Property(cs => cs.Amount).HasColumnType("decimal(18,2)");
            e.Property(cs => cs.Description).HasMaxLength(1000);
            e.Property(cs => cs.SettlUser).HasMaxLength(100);
            e.Property(cs => cs.Branch).HasMaxLength(50);
        });

        // Account → Accounts table
        modelBuilder.Entity<Account>(e =>
        {
            e.ToTable("Accounts");
            e.HasKey(a => a.AccountNo);
            e.Property(a => a.AccountNo).HasMaxLength(50);
            e.Property(a => a.AccountName).HasMaxLength(200);
            e.Property(a => a.ParentAcc).HasMaxLength(50);
            e.Property(a => a.FullPath).HasMaxLength(500);
            e.Property(a => a.Branch).HasMaxLength(50);
        });

        // MainJournal → Journal table
        modelBuilder.Entity<MainJournal>(e =>
        {
            e.ToTable("Journal");
            e.HasKey(j => j.DailyNum);
            e.Property(j => j.DailyNum).HasMaxLength(50);
            e.Property(j => j.AnalysisNum).HasMaxLength(50);
            e.Property(j => j.Comment).HasMaxLength(500);
            e.Property(j => j.Exchange).HasColumnType("decimal(18,6)");
            e.Property(j => j.CurUser).HasMaxLength(100);
            e.Property(j => j.RecNo).HasMaxLength(50);
            e.Property(j => j.Branch).HasMaxLength(50);
            e.HasMany(j => j.Details).WithOne().HasForeignKey(d => d.DailyNum);
        });

        // JournalDetail → JournalDetails table
        modelBuilder.Entity<JournalDetail>(e =>
        {
            e.ToTable("JournalDetails");
            e.HasKey(d => d.Id);
            e.Property(d => d.DailyNum).HasMaxLength(50);
            e.Property(d => d.AccountNo).HasMaxLength(50);
            e.Property(d => d.Dr).HasColumnType("decimal(18,2)");
            e.Property(d => d.Cr).HasColumnType("decimal(18,2)");
            e.Property(d => d.CurUser).HasMaxLength(100);
            e.Property(d => d.Branch).HasMaxLength(50);
            e.Property(d => d.Comment).HasMaxLength(500);
        });

        // Receipt → ReceiptFile table
        modelBuilder.Entity<Receipt>(e =>
        {
            e.ToTable("ReceiptFile");
            e.HasKey(r => r.DocNo);
            e.Property(r => r.DocNo).HasMaxLength(50);
            e.Property(r => r.SubDocNo).HasMaxLength(50);
            e.Property(r => r.CustName).HasMaxLength(200);
            e.Property(r => r.Payment).HasColumnType("decimal(18,2)");
            e.Property(r => r.Amount).HasColumnType("decimal(18,2)");
            e.Property(r => r.ForW).HasMaxLength(50);
            e.Property(r => r.Type).HasMaxLength(20);
            e.Property(r => r.Branch).HasMaxLength(50);
            e.Property(r => r.AccNo).HasMaxLength(50);
            e.Property(r => r.Bank).HasMaxLength(100);
            e.Property(r => r.Currency).HasMaxLength(10);
            e.Property(r => r.Node).HasMaxLength(200);
            e.Property(r => r.PayType).HasMaxLength(20);
            e.Property(r => r.UserName).HasMaxLength(100);
            e.Property(r => r.Note).HasMaxLength(500);
            e.Property(r => r.PaymentDetail).HasMaxLength(500);
            e.Property(r => r.AccountUsed).HasMaxLength(50);
        });

        // Treaty → Treaties table
        modelBuilder.Entity<Treaty>(e =>
        {
            e.ToTable("Treaties");
            e.HasKey(t => t.Id);
            e.Property(t => t.TreatyName).HasMaxLength(200);
            e.Property(t => t.TreatyNo).HasMaxLength(50);
            e.Property(t => t.SubIns).HasMaxLength(20);
            e.Property(t => t.RetentionPercent).HasColumnType("decimal(18,4)");
            e.Property(t => t.RetentionAmount).HasColumnType("decimal(18,2)");
            e.Property(t => t.Capacity).HasColumnType("decimal(18,2)");
            e.Property(t => t.ReinsurerId).HasMaxLength(50);
            e.Property(t => t.ReinsurerName).HasMaxLength(200);
            e.Property(t => t.SharePercent).HasColumnType("decimal(18,4)");
            e.Property(t => t.Branch).HasMaxLength(50);
            e.Property(t => t.Note).HasMaxLength(1000);
        });

        // SubSystem → SubSystem table
        modelBuilder.Entity<SubSystem>(e =>
        {
            e.ToTable("SubSystem");
            e.HasKey(s => s.SubSysNo);
            e.Property(s => s.SubSysNo).HasMaxLength(20);
            e.Property(s => s.SubSysName).HasMaxLength(200);
            e.Property(s => s.MainSys).HasMaxLength(20);
            e.Property(s => s.Branch).HasMaxLength(50);
            e.Property(s => s.EditForm).HasMaxLength(200);
            e.Property(s => s.ExtraInfo).HasMaxLength(500);
            e.Property(s => s.GroupFile).HasMaxLength(200);
            e.Property(s => s.EndFile).HasMaxLength(200);
            e.Property(s => s.IssuVal).HasColumnType("decimal(18,4)");
            e.Property(s => s.StmVal).HasColumnType("decimal(18,4)");
            e.Property(s => s.WakalaVal).HasColumnType("decimal(18,4)");
        });

        // Notification
        modelBuilder.Entity<Notification>(e =>
        {
            e.ToTable("Notifications");
            e.HasKey(n => n.Id);
            e.Property(n => n.Title).HasMaxLength(200);
            e.Property(n => n.Message).HasMaxLength(2000);
            e.Property(n => n.Type).HasMaxLength(50);
            e.Property(n => n.TargetUser).HasMaxLength(100);
            e.Property(n => n.RelatedEntity).HasMaxLength(50);
            e.Property(n => n.RelatedId).HasMaxLength(50);
        });

        // LogEntry → LogFile table
        modelBuilder.Entity<LogEntry>(e =>
        {
            e.ToTable("LogFile");
            e.HasKey(l => l.Id);
            e.Property(l => l.UserName).HasMaxLength(100);
            e.Property(l => l.Operation).HasMaxLength(500);
            e.Property(l => l.IPAddress).HasMaxLength(50);
        });
    }
}
