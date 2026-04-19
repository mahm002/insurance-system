using InsuranceAPI.Application.DTOs.Admin;
using InsuranceAPI.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceAPI.WebAPI.Controllers;

[ApiController]
[Route("api/admin/[controller]")]
[Authorize]
public class BranchesController : ControllerBase
{
    private readonly IBranchService _branchService;

    public BranchesController(IBranchService branchService)
    {
        _branchService = branchService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _branchService.GetAllAsync(page, pageSize);
        return Ok(result);
    }

    [HttpGet("{branchNo}")]
    public async Task<IActionResult> GetById(string branchNo)
    {
        var result = await _branchService.GetByIdAsync(branchNo);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBranchRequest request)
    {
        var result = await _branchService.CreateAsync(request);
        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { branchNo = result.Data!.BranchNo }, result);
    }

    [HttpPut("{branchNo}")]
    public async Task<IActionResult> Update(string branchNo, [FromBody] UpdateBranchRequest request)
    {
        var result = await _branchService.UpdateAsync(branchNo, request);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpDelete("{branchNo}")]
    public async Task<IActionResult> Delete(string branchNo)
    {
        var result = await _branchService.DeleteAsync(branchNo);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }
}
