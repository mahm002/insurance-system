namespace InsuranceAPI.Application.DTOs.Admin;

// ── Users ──
public class UserDto
{
    public int AccountNo { get; set; }
    public string AccountLogIn { get; set; } = string.Empty;
    public string? AccountName { get; set; }
    public string Branch { get; set; } = string.Empty;
    public string AccountPermSys { get; set; } = string.Empty;
    public string AccountPermClm { get; set; } = string.Empty;
    public string AccountPermFin { get; set; } = string.Empty;
    public string AccountPermRe { get; set; } = string.Empty;
    public string AccountPermMan { get; set; } = string.Empty;
    public string? AccountSysManag { get; set; }
    public double? AccLimit { get; set; }
    public bool Stop { get; set; }
}

public class CreateUserRequest
{
    public string AccountLogIn { get; set; } = string.Empty;
    public string? AccountName { get; set; }
    public string Password { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;
    public string AccountPermSys { get; set; } = string.Empty;
    public string AccountPermClm { get; set; } = string.Empty;
    public string AccountPermFin { get; set; } = string.Empty;
    public string AccountPermRe { get; set; } = string.Empty;
    public string AccountPermMan { get; set; } = string.Empty;
    public string? AccountSysManag { get; set; }
    public double? AccLimit { get; set; }
}

public class UpdateUserRequest
{
    public string? AccountName { get; set; }
    public string Branch { get; set; } = string.Empty;
    public string AccountPermSys { get; set; } = string.Empty;
    public string AccountPermClm { get; set; } = string.Empty;
    public string AccountPermFin { get; set; } = string.Empty;
    public string AccountPermRe { get; set; } = string.Empty;
    public string AccountPermMan { get; set; } = string.Empty;
    public string? AccountSysManag { get; set; }
    public double? AccLimit { get; set; }
    public bool Stop { get; set; }
}

// ── Branches ──
public class BranchDto
{
    public string BranchNo { get; set; } = string.Empty;
    public string BranchCode { get; set; } = string.Empty;
    public bool? Agent { get; set; }
    public string? BranchName { get; set; }
    public string? BranchNameEn { get; set; }
    public string? Address { get; set; }
    public string? Telephone { get; set; }
    public string? FaxNo { get; set; }
    public string? EMail { get; set; }
    public long ManagerId { get; set; }
    public string? CashierAccount { get; set; }
    public string? ChequeAccount { get; set; }
    public string AccountingCode { get; set; } = string.Empty;
    public bool MainCenter { get; set; }
    public bool CompanyOffice { get; set; }
}

public class CreateBranchRequest
{
    public string BranchNo { get; set; } = string.Empty;
    public string BranchCode { get; set; } = string.Empty;
    public string? BranchName { get; set; }
    public string? BranchNameEn { get; set; }
    public string? Address { get; set; }
    public string? Telephone { get; set; }
    public string? FaxNo { get; set; }
    public string? EMail { get; set; }
    public long ManagerId { get; set; }
    public string AccountingCode { get; set; } = string.Empty;
}

public class UpdateBranchRequest
{
    public string? BranchName { get; set; }
    public string? BranchNameEn { get; set; }
    public string? Address { get; set; }
    public string? Telephone { get; set; }
    public string? FaxNo { get; set; }
    public string? EMail { get; set; }
    public long ManagerId { get; set; }
    public string? CashierAccount { get; set; }
    public string? ChequeAccount { get; set; }
    public string AccountingCode { get; set; } = string.Empty;
}

// ── Customers ──
public class CustomerDto
{
    public long CustNo { get; set; }
    public int CustTP { get; set; }
    public string CustName { get; set; } = string.Empty;
    public string CustNameE { get; set; } = string.Empty;
    public string IDNo { get; set; } = string.Empty;
    public string DrCardNo { get; set; } = string.Empty;
    public string TelNo { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int? SpecialCase { get; set; }
    public string? AccNo { get; set; }
}

public class CreateCustomerRequest
{
    public long CustNo { get; set; }
    public int CustTP { get; set; }
    public string CustName { get; set; } = string.Empty;
    public string CustNameE { get; set; } = string.Empty;
    public string IDNo { get; set; } = string.Empty;
    public string DrCardNo { get; set; } = string.Empty;
    public string TelNo { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int? SpecialCase { get; set; }
}

public class UpdateCustomerRequest
{
    public string CustName { get; set; } = string.Empty;
    public string CustNameE { get; set; } = string.Empty;
    public string IDNo { get; set; } = string.Empty;
    public string DrCardNo { get; set; } = string.Empty;
    public string TelNo { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int? SpecialCase { get; set; }
}

// ── Agent Commissions ──
public class AgentCommissionDto
{
    public long Id { get; set; }
    public string AgentNo { get; set; } = string.Empty;
    public string SubIns { get; set; } = string.Empty;
    public decimal Comm { get; set; }
    public string AccountNo { get; set; } = string.Empty;
}

public class CreateAgentCommissionRequest
{
    public string AgentNo { get; set; } = string.Empty;
    public string SubIns { get; set; } = string.Empty;
    public decimal Comm { get; set; }
    public string AccountNo { get; set; } = string.Empty;
}

public class UpdateAgentCommissionRequest
{
    public decimal Comm { get; set; }
    public string AccountNo { get; set; } = string.Empty;
}
