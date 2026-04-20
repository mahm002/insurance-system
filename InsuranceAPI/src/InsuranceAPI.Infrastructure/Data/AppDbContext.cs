using InsuranceAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InsuranceAPI.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<AgentCommission> AgentCommissions => Set<AgentCommission>();
    public DbSet<Policy> Policies => Set<Policy>();
    public DbSet<Claim> Claims => Set<Claim>();
    public DbSet<ClaimEstimation> ClaimEstimations => Set<ClaimEstimation>();
    public DbSet<ClaimSettlement> ClaimSettlements => Set<ClaimSettlement>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<MainJournal> MainJournals => Set<MainJournal>();
    public DbSet<JournalDetail> JournalDetails => Set<JournalDetail>();
    public DbSet<Receipt> Receipts => Set<Receipt>();
    public DbSet<LogEntry> LogEntries => Set<LogEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User → AccountFile table
        // PK is AccountLogIn (nvarchar(50)), AccountNo is IDENTITY auto-increment
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("AccountFile");
            e.HasKey(u => u.AccountLogIn);
            e.Property(u => u.AccountNo).ValueGeneratedOnAdd();
            e.Property(u => u.AccountLogIn).HasMaxLength(50);
            e.Property(u => u.AccountName).HasMaxLength(100);
            e.Property(u => u.AccountPassWord).HasMaxLength(100);
            e.Property(u => u.AccountPermSys).HasMaxLength(600);
            e.Property(u => u.AccountPermClm).HasMaxLength(600);
            e.Property(u => u.AccountPermFin).HasMaxLength(600);
            e.Property(u => u.AccountPermRe).HasMaxLength(600);
            e.Property(u => u.AccountPermMan).HasMaxLength(600);
            e.Property(u => u.AccountSysManag).HasMaxLength(600);
            e.Property(u => u.Branch).HasColumnType("nchar(5)");
            e.Property(u => u.AddedBy).HasMaxLength(100);
            e.Property(u => u.ModifiedBy).HasMaxLength(100);
        });

        // Branch → BranchInfo table
        modelBuilder.Entity<Branch>(e =>
        {
            e.ToTable("BranchInfo");
            e.HasKey(b => b.BranchNo);
            e.Property(b => b.BranchNo).HasColumnType("nchar(5)");
            e.Property(b => b.BranchCode).HasColumnType("nchar(6)");
            e.Property(b => b.BranchName).HasMaxLength(80);
            e.Property(b => b.BranchNameEn).HasMaxLength(80);
            e.Property(b => b.Address).HasColumnName("address").HasMaxLength(200);
            e.Property(b => b.Telephone).HasMaxLength(30);
            e.Property(b => b.FaxNo).HasMaxLength(30);
            e.Property(b => b.EMail).HasColumnName("eMail").HasMaxLength(30);
            e.Property(b => b.AccountingCode).HasMaxLength(2);
            e.Property(b => b.SystemURI).HasColumnType("nchar(100)");
            e.Property(b => b.CashierAccount).HasMaxLength(30);
            e.Property(b => b.ChequeAccount).HasMaxLength(30);
        });

        // Customer → CustomerFile table
        modelBuilder.Entity<Customer>(e =>
        {
            e.ToTable("CustomerFile");
            e.HasKey(c => c.CustNo);
            e.Property(c => c.Key).ValueGeneratedOnAdd();
            e.Property(c => c.CustName).HasMaxLength(200);
            e.Property(c => c.CustNameE).HasMaxLength(200);
            e.Property(c => c.IDNo).HasColumnType("nchar(20)");
            e.Property(c => c.DrCardNo).HasColumnType("nchar(20)");
            e.Property(c => c.CustNat).HasColumnType("nchar(20)");
            e.Property(c => c.TelNo).HasColumnType("nchar(14)");
            e.Property(c => c.FaxNo).HasColumnType("nchar(14)");
            e.Property(c => c.Address).HasMaxLength(100);
            e.Property(c => c.Email).HasColumnType("nchar(50)");
            e.Property(c => c.AccNo).HasMaxLength(50);
            e.Property(c => c.BankAcc).HasMaxLength(20);
            e.Property(c => c.OldCust).HasMaxLength(100);
            e.Property(c => c.UserName).HasMaxLength(100);
        });

        // AgentCommission → AgentsCommisions table
        // PK: (AgentNo, SubIns) composite key
        modelBuilder.Entity<AgentCommission>(e =>
        {
            e.ToTable("AgentsCommisions");
            e.HasKey(a => new { a.AgentNo, a.SubIns });
            e.Property(a => a.Id).ValueGeneratedOnAdd();
            e.Property(a => a.AgentNo).HasMaxLength(6);
            e.Property(a => a.SubIns).HasMaxLength(2);
            e.Property(a => a.Comm).HasColumnType("decimal(18, 3)");
            e.Property(a => a.AccountNo).HasMaxLength(30);
        });

        // Policy → PolicyFile table
        // Note: PolicyFile schema was not in the provided DB script (truncated).
        // This mapping is based on VB.NET code analysis and will be validated later.
        modelBuilder.Entity<Policy>(e =>
        {
            e.ToTable("PolicyFile");
            e.HasKey(p => new { p.OrderNo, p.EndNo, p.LoadNo, p.SubIns });
            e.Property(p => p.OrderNo).HasMaxLength(50);
            e.Property(p => p.PolNo).HasMaxLength(50);
            e.Property(p => p.SubIns).HasMaxLength(20);
            e.Property(p => p.Branch).HasMaxLength(50);
            e.Property(p => p.IssueUser).HasMaxLength(100);
            e.Property(p => p.CurrencyCode).HasMaxLength(10);
            e.HasOne(p => p.Customer).WithMany().HasForeignKey(p => p.CustNo).OnDelete(DeleteBehavior.NoAction);
            e.HasOne(p => p.BranchInfo).WithMany().HasForeignKey(p => p.Branch).OnDelete(DeleteBehavior.NoAction);
        });

        // Claim → MainClaimFile table
        // PK: (ClmNo, SubIns, Branch) composite key
        modelBuilder.Entity<Claim>(e =>
        {
            e.ToTable("MainClaimFile");
            e.HasKey(c => new { c.ClmNo, c.SubIns, c.Branch });
            e.Property(c => c.ClmNo).HasColumnType("nchar(30)");
            e.Property(c => c.PolNo).HasColumnType("nchar(30)");
            e.Property(c => c.SubIns).HasColumnType("nchar(2)");
            e.Property(c => c.Branch).HasColumnType("nchar(6)");
            e.Property(c => c.InfName).HasColumnType("nchar(200)");
            e.Property(c => c.PrevName).HasColumnType("nchar(200)");
            e.Property(c => c.ClmPlace).HasColumnType("nchar(200)");
            e.Property(c => c.ClmReason).HasColumnType("nchar(200)");
            e.Property(c => c.UserPc).HasColumnType("nchar(400)");
            e.Property(c => c.UserName).HasColumnType("nchar(200)");
        });

        // ClaimEstimation → Estimation table
        // PK: (TPID, ClmNo, Date) composite key
        modelBuilder.Entity<ClaimEstimation>(e =>
        {
            e.ToTable("Estimation");
            e.HasKey(ce => new { ce.TPID, ce.ClmNo, ce.Date });
            e.Property(ce => ce.ClmNo).HasColumnType("nchar(25)");
            e.Property(ce => ce.PolNo).HasColumnType("nchar(30)");
        });

        // ClaimSettlement → MainSattelement table
        // PK: (ClmNo, TPID, No) composite key
        modelBuilder.Entity<ClaimSettlement>(e =>
        {
            e.ToTable("MainSattelement");
            e.HasKey(cs => new { cs.ClmNo, cs.TPID, cs.No });
            e.Property(cs => cs.ClmNo).HasMaxLength(17);
            e.Property(cs => cs.PayTo).HasMaxLength(100);
            e.Property(cs => cs.DAILYNUM).HasMaxLength(10);
            e.Property(cs => cs.UserName).HasMaxLength(100);
            e.Property(cs => cs.SerNo).ValueGeneratedOnAdd();
        });

        // Account → Accounts table
        modelBuilder.Entity<Account>(e =>
        {
            e.ToTable("Accounts");
            e.HasKey(a => a.AccountNo);
            e.Property(a => a.AccountNo).HasMaxLength(40);
            e.Property(a => a.AccountName).HasMaxLength(255);
            e.Property(a => a.ParentAcc).HasMaxLength(40);
            e.Property(a => a.FullPath).HasMaxLength(2000);
            // Note: DB column is "TranscationAcc" (typo preserved)
            e.Property(a => a.TranscationAcc).HasColumnName("TranscationAcc");
        });

        // MainJournal → MainJournal table (header records)
        // PK: (DAILYNUM, DailyTyp, Sn) composite key
        modelBuilder.Entity<MainJournal>(e =>
        {
            e.ToTable("MainJournal");
            e.HasKey(j => new { j.DAILYNUM, j.DailyTyp, j.Sn });
            e.Property(j => j.DAILYNUM).HasMaxLength(12);
            e.Property(j => j.DAILYSRL).HasMaxLength(12);
            e.Property(j => j.ANALSNUM).HasMaxLength(3);
            e.Property(j => j.PayedFor).HasMaxLength(400);
            e.Property(j => j.CurUser).HasMaxLength(200);
            e.Property(j => j.UpUser).HasMaxLength(200);
            e.Property(j => j.RecNo).HasMaxLength(50);
            e.Property(j => j.Branch).HasMaxLength(6);
            e.Property(j => j.SubBranch).HasMaxLength(6);
            e.Property(j => j.Sn).ValueGeneratedOnAdd();
        });

        // JournalDetail → Journal table (detail/line records)
        // PK: (DAILYNUM, TP, Idx) composite key
        modelBuilder.Entity<JournalDetail>(e =>
        {
            e.ToTable("Journal");
            e.HasKey(d => new { d.DAILYNUM, d.TP, d.Idx });
            e.Property(d => d.DAILYNUM).HasMaxLength(50);
            e.Property(d => d.AccountNo).HasMaxLength(40);
            e.Property(d => d.Dr).HasColumnType("decimal(20, 3)");
            e.Property(d => d.Cr).HasColumnType("decimal(20, 3)");
            e.Property(d => d.DocNum).HasMaxLength(50);
            e.Property(d => d.CustName).HasMaxLength(100);
            e.Property(d => d.CurUser).HasMaxLength(200);
            e.Property(d => d.Branch).HasMaxLength(6);
            e.Property(d => d.SubBranch).HasMaxLength(6);
            e.Property(d => d.Idx).HasColumnName("idx").ValueGeneratedOnAdd();
        });

        // Receipt → ACCMOVE table
        modelBuilder.Entity<Receipt>(e =>
        {
            e.ToTable("ACCMOVE");
            e.HasKey(r => r.SerNo);
            e.Property(r => r.SerNo).ValueGeneratedOnAdd();
            e.Property(r => r.DocNo).HasMaxLength(20);
            e.Property(r => r.SubDocNo).HasMaxLength(30);
            e.Property(r => r.CustNAme).HasColumnName("CustNAme").HasMaxLength(200);
            e.Property(r => r.AccNo).HasMaxLength(100);
            e.Property(r => r.Tp).HasMaxLength(100);
            e.Property(r => r.Node).HasColumnType("char(1)");
            e.Property(r => r.Cur).HasMaxLength(40);
            e.Property(r => r.Branch).HasMaxLength(100);
            e.Property(r => r.PaymentDetail).HasMaxLength(50);
            e.Property(r => r.AccountUsed).HasMaxLength(50);
        });

        // LogEntry → LogData table
        modelBuilder.Entity<LogEntry>(e =>
        {
            e.ToTable("LogData");
            e.HasKey(l => l.Id);
            e.Property(l => l.Id).ValueGeneratedOnAdd();
            e.Property(l => l.UserName).HasMaxLength(250);
            e.Property(l => l.IPAddress).HasMaxLength(15);
        });
    }
}
