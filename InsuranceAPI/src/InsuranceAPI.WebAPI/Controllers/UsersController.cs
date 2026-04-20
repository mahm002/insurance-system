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

    [HttpGet("{accountLogIn}")]
    public async Task<IActionResult> GetById(string accountLogIn)
    {
        var result = await _userService.GetByIdAsync(accountLogIn);
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

        return CreatedAtAction(nameof(GetById), new { accountLogIn = result.Data!.AccountLogIn }, result);
    }

    [HttpPut("{accountLogIn}")]
    public async Task<IActionResult> Update(string accountLogIn, [FromBody] UpdateUserRequest request)
    {
        var result = await _userService.UpdateAsync(accountLogIn, request);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpDelete("{accountLogIn}")]
    public async Task<IActionResult> Delete(string accountLogIn)
    {
        var result = await _userService.DeleteAsync(accountLogIn);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPost("{accountLogIn}/reset-password")]
    public async Task<IActionResult> ResetPassword(string accountLogIn, [FromBody] ResetPasswordRequest request)
    {
        var result = await _userService.ResetPasswordAsync(accountLogIn, request.NewPassword);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }
}

public class ResetPasswordRequest
{
    public string NewPassword { get; set; } = string.Empty;
}
