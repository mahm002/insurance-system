using InsuranceAPI.Application.DTOs.Admin;
using InsuranceAPI.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceAPI.WebAPI.Controllers;

[ApiController]
[Route("api/admin/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _userService.GetAllAsync(page, pageSize);
        return Ok(result);
    }

    [HttpGet("{accountNo}")]
    public async Task<IActionResult> GetById(int accountNo)
    {
        var result = await _userService.GetByIdAsync(accountNo);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var result = await _userService.CreateAsync(request);
        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { accountNo = result.Data!.AccountNo }, result);
    }

    [HttpPut("{accountNo}")]
    public async Task<IActionResult> Update(int accountNo, [FromBody] UpdateUserRequest request)
    {
        var result = await _userService.UpdateAsync(accountNo, request);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpDelete("{accountNo}")]
    public async Task<IActionResult> Delete(int accountNo)
    {
        var result = await _userService.DeleteAsync(accountNo);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPost("{accountNo}/reset-password")]
    public async Task<IActionResult> ResetPassword(int accountNo, [FromBody] ResetPasswordRequest request)
    {
        var result = await _userService.ResetPasswordAsync(accountNo, request.NewPassword);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }
}

public class ResetPasswordRequest
{
    public string NewPassword { get; set; } = string.Empty;
}
