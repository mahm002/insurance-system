using System.Security.Claims;
using InsuranceAPI.Application.DTOs.Auth;
using InsuranceAPI.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceAPI.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);
        if (!result.Success)
            return Unauthorized(result);

        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result = await _authService.RefreshTokenAsync(request);
        if (!result.Success)
            return Unauthorized(result);

        return Ok(result);
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var accountNoStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(accountNoStr, out var accountNo))
            return Unauthorized();

        var result = await _authService.ChangePasswordAsync(accountNo, request);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        var userInfo = new UserInfo
        {
            AccountNo = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0"),
            AccountName = User.FindFirstValue("AccountName") ?? "",
            Username = User.FindFirstValue(ClaimTypes.Name) ?? "",
            Branch = User.FindFirstValue("Branch"),
            Permissions = new PermissionsInfo
            {
                System = int.Parse(User.FindFirstValue("PermSys") ?? "0"),
                Claims = int.Parse(User.FindFirstValue("PermClm") ?? "0"),
                Finance = int.Parse(User.FindFirstValue("PermFin") ?? "0"),
                Reinsurance = int.Parse(User.FindFirstValue("PermRe") ?? "0"),
                Management = int.Parse(User.FindFirstValue("PermMan") ?? "0"),
                SysAdmin = int.Parse(User.FindFirstValue("SysManag") ?? "0")
            }
        };

        return Ok(new { Success = true, Data = userInfo });
    }
}
