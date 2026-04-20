using InsuranceAPI.Application.DTOs.Admin;
using InsuranceAPI.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceAPI.WebAPI.Controllers;

[ApiController]
[Route("api/admin/[controller]")]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null)
    {
        var result = await _customerService.GetAllAsync(page, pageSize, search);
        return Ok(result);
    }

    [HttpGet("{custNo:long}")]
    public async Task<IActionResult> GetById(long custNo)
    {
        var result = await _customerService.GetByIdAsync(custNo);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request)
    {
        var result = await _customerService.CreateAsync(request);
        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { custNo = result.Data!.CustNo }, result);
    }

    [HttpPut("{custNo:long}")]
    public async Task<IActionResult> Update(long custNo, [FromBody] UpdateCustomerRequest request)
    {
        var result = await _customerService.UpdateAsync(custNo, request);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpDelete("{custNo:long}")]
    public async Task<IActionResult> Delete(long custNo)
    {
        var result = await _customerService.DeleteAsync(custNo);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }
}
