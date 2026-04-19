namespace InsuranceAPI.Application.DTOs.Admin;

// ── Users ──
public class UserDto
{
    public int AccountNo { get; set; }
    public string AccountLogIn { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string? Branch { get; set; }
    public int AccountPermSys { get; set; }
    public int AccountPermClm { get; set; }
    public int AccountPermFin { get; set; }
    public int AccountPermRe { get; set; }
    public int AccountPermMan { get; set; }
    public int AccountSysManag { get; set; }
    public decimal AccLimit { get; set; }
    public bool IsActive { get; set; }
}

public class CreateUserRequest
{
    public string AccountLogIn { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Branch { get; set; }
    public int AccountPermSys { get; set; }
    public int AccountPermClm { get; set; }
    public int AccountPermFin { get; set; }
    public int AccountPermRe { get; set; }
    public int AccountPermMan { get; set; }
    public int AccountSysManag { get; set; }
    public decimal AccLimit { get; set; }
}

public class UpdateUserRequest
{
    public string AccountName { get; set; } = string.Empty;
    public string? Branch { get; set; }
    public int AccountPermSys { get; set; }
    public int AccountPermClm { get; set; }
    public int AccountPermFin { get; set; }
    public int AccountPermRe { get; set; }
    public int AccountPermMan { get; set; }
    public int AccountSysManag { get; set; }
    public decimal AccLimit { get; set; }
    public bool IsActive { get; set; }
}

// ── Branches ──
public class BranchDto
{
    public string BranchNo { get; set; } = string.Empty;
    public string? BranchName { get; set; }
    public string? BranchNameE { get; set; }
    public string? Address { get; set; }
    public string? TelNo { get; set; }
    public string? FaxNo { get; set; }
    public string? Email { get; set; }
    public int? ManagerId { get; set; }
    public string? CashierAccount { get; set; }
    public string? ChequeAccount { get; set; }
    public int? AccountingCode { get; set; }
    public bool IsActive { get; set; }
}

public class CreateBranchRequest
{
    public string BranchNo { get; set; } = string.Empty;
    public string? BranchName { get; set; }
    public string? BranchNameE { get; set; }
    public string? Address { get; set; }
    public string? TelNo { get; set; }
    public string? FaxNo { get; set; }
    public string? Email { get; set; }
    public int? ManagerId { get; set; }
}

public class UpdateBranchRequest
{
    public string? BranchName { get; set; }
    public string? BranchNameE { get; set; }
    public string? Address { get; set; }
    public string? TelNo { get; set; }
    public string? FaxNo { get; set; }
    public string? Email { get; set; }
    public int? ManagerId { get; set; }
    public string? CashierAccount { get; set; }
    public string? ChequeAccount { get; set; }
    public bool IsActive { get; set; }
}

// ── Customers ──
public class CustomerDto
{
    public int CustNo { get; set; }
    public string CustName { get; set; } = string.Empty;
    public string? CustNameE { get; set; }
    public string? TelNo { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public int? SpecialCase { get; set; }
    public string? AccNo { get; set; }
    public string? Branch { get; set; }
    public string? NationalId { get; set; }
    public string? PassportNo { get; set; }
    public DateTime? DateOfBirth { get; set; }
}

public class CreateCustomerRequest
{
    public string CustName { get; set; } = string.Empty;
    public string? CustNameE { get; set; }
    public string? TelNo { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public int? SpecialCase { get; set; }
    public string? Branch { get; set; }
    public string? NationalId { get; set; }
    public string? PassportNo { get; set; }
    public DateTime? DateOfBirth { get; set; }
}

public class UpdateCustomerRequest
{
    public string CustName { get; set; } = string.Empty;
    public string? CustNameE { get; set; }
    public string? TelNo { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public int? SpecialCase { get; set; }
    public string? Branch { get; set; }
    public string? NationalId { get; set; }
    public string? PassportNo { get; set; }
    public DateTime? DateOfBirth { get; set; }
}

// ── Agents ──
public class AgentDto
{
    public int AgentNo { get; set; }
    public string AgentName { get; set; } = string.Empty;
    public string? AgentNameE { get; set; }
    public string? Address { get; set; }
    public string? TelNo { get; set; }
    public string? Email { get; set; }
    public string? Branch { get; set; }
    public decimal? CommissionRate { get; set; }
    public string? AccNo { get; set; }
    public bool IsActive { get; set; }
}

public class CreateAgentRequest
{
    public string AgentName { get; set; } = string.Empty;
    public string? AgentNameE { get; set; }
    public string? Address { get; set; }
    public string? TelNo { get; set; }
    public string? Email { get; set; }
    public string? Branch { get; set; }
    public decimal? CommissionRate { get; set; }
}

public class UpdateAgentRequest
{
    public string AgentName { get; set; } = string.Empty;
    public string? AgentNameE { get; set; }
    public string? Address { get; set; }
    public string? TelNo { get; set; }
    public string? Email { get; set; }
    public string? Branch { get; set; }
    public decimal? CommissionRate { get; set; }
    public bool IsActive { get; set; }
}
