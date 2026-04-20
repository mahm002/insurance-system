namespace InsuranceAPI.Application.DTOs.Auth;

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public UserInfo User { get; set; } = new();
}

public class UserInfo
{
    public int AccountNo { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string? Branch { get; set; }
    public PermissionsInfo Permissions { get; set; } = new();
}

public class PermissionsInfo
{
    public string System { get; set; } = string.Empty;
    public string Claims { get; set; } = string.Empty;
    public string Finance { get; set; } = string.Empty;
    public string Reinsurance { get; set; } = string.Empty;
    public string Management { get; set; } = string.Empty;
    public string? SysAdmin { get; set; }
}

public class ChangePasswordRequest
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

public class RefreshTokenRequest
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
