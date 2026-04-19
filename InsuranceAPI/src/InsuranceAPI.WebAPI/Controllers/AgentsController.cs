using InsuranceAPI.Application.DTOs.Admin;
using InsuranceAPI.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceAPI.WebAPI.Controllers;

[ApiController]
[Route("api/admin/[controller]")]
[Authorize]
public class AgentsController : ControllerBase
{
    private readonly IAgentService _agentService;

    public AgentsController(IAgentService agentService)
    {
        _agentService = agentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _agentService.GetAllAsync(page, pageSize);
        return Ok(result);
    }

    [HttpGet("{agentNo}")]
    public async Task<IActionResult> GetById(int agentNo)
    {
        var result = await _agentService.GetByIdAsync(agentNo);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAgentRequest request)
    {
        var result = await _agentService.CreateAsync(request);
        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { agentNo = result.Data!.AgentNo }, result);
    }

    [HttpPut("{agentNo}")]
    public async Task<IActionResult> Update(int agentNo, [FromBody] UpdateAgentRequest request)
    {
        var result = await _agentService.UpdateAsync(agentNo, request);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpDelete("{agentNo}")]
    public async Task<IActionResult> Delete(int agentNo)
    {
        var result = await _agentService.DeleteAsync(agentNo);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }
}
