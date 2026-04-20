using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using InsuranceAPI.Application.DTOs.Auth;
using InsuranceAPI.Application.DTOs.Common;
using InsuranceAPI.Application.Services.Interfaces;
using InsuranceAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace InsuranceAPI.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<ApiResult<LoginResponse>> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.AccountLogIn == request.Username && !u.Stop);

        if (user == null)
            return ApiResult<LoginResponse>.Fail("Invalid username or password.");

        if (string.IsNullOrEmpty(user.AccountPassWord))
            return ApiResult<LoginResponse>.Fail("Account password not set.");

        bool passwordValid;
        try
        {
            passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.AccountPassWord);
        }
        catch
        {
            // Fallback: direct comparison for legacy non-hashed passwords
            passwordValid = user.AccountPassWord == request.Password;
        }

        if (!passwordValid)
            return ApiResult<LoginResponse>.Fail("Invalid username or password.");

        var token = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();

        var response = new LoginResponse
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(
                _configuration.GetValue("Jwt:ExpiryHours", 8)),
            User = new UserInfo
            {
                AccountNo = user.AccountNo,
                AccountName = user.AccountName ?? "",
                Username = user.AccountLogIn,
                Branch = user.Branch,
                Permissions = new PermissionsInfo
                {
                    System = user.AccountPermSys,
                    Claims = user.AccountPermClm,
                    Finance = user.AccountPermFin,
                    Reinsurance = user.AccountPermRe,
                    Management = user.AccountPermMan,
                    SysAdmin = user.AccountSysManag
                }
            }
        };

        return ApiResult<LoginResponse>.Ok(response, "Login successful.");
    }

    public Task<ApiResult<LoginResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        // Placeholder for refresh token logic (requires token storage)
        return Task.FromResult(ApiResult<LoginResponse>.Fail("Refresh token not implemented yet."));
    }

    public async Task<ApiResult<bool>> ChangePasswordAsync(string accountLogIn, ChangePasswordRequest request)
    {
        var user = await _context.Users.FindAsync(accountLogIn);
        if (user == null)
            return ApiResult<bool>.Fail("User not found.");

        bool currentPasswordValid;
        try
        {
            currentPasswordValid = BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.AccountPassWord);
        }
        catch
        {
            currentPasswordValid = user.AccountPassWord == request.CurrentPassword;
        }

        if (!currentPasswordValid)
            return ApiResult<bool>.Fail("Current password is incorrect.");

        user.AccountPassWord = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.ModifiedBy = accountLogIn;
        user.ModifyDate = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return ApiResult<bool>.Ok(true, "Password changed successfully.");
    }

    private string GenerateJwtToken(Domain.Entities.User user)
    {
        var jwtKeyValue = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("JWT Key must be configured via Jwt:Key.");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKeyValue));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.AccountNo.ToString()),
            new(ClaimTypes.Name, user.AccountLogIn),
            new("AccountName", user.AccountName ?? ""),
            new("Branch", user.Branch),
            new("PermSys", user.AccountPermSys),
            new("PermClm", user.AccountPermClm),
            new("PermFin", user.AccountPermFin),
            new("PermRe", user.AccountPermRe),
            new("PermMan", user.AccountPermMan),
            new("SysManag", user.AccountSysManag ?? ""),
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"] ?? "InsuranceAPI",
            audience: _configuration["Jwt:Audience"] ?? "InsuranceAPI",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(_configuration.GetValue("Jwt:ExpiryHours", 8)),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
