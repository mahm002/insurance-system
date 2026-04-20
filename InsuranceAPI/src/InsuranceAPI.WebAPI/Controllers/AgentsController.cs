using InsuranceAPI.Application.DTOs.Admin;
using InsuranceAPI.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceAPI.WebAPI.Controllers;

[ApiController]
[Route("api/admin/agent-commissions")]
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

    [HttpGet("{agentNo}/{subIns}")]
    public async Task<IActionResult> GetById(string agentNo, string subIns)
    {
        var result = await _agentService.GetByIdAsync(agentNo, subIns);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAgentCommissionRequest request)
    {
        var result = await _agentService.CreateAsync(request);
        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById),
            new { agentNo = result.Data!.AgentNo, subIns = result.Data.SubIns }, result);
    }

    [HttpPut("{agentNo}/{subIns}")]
    public async Task<IActionResult> Update(string agentNo, string subIns, [FromBody] UpdateAgentCommissionRequest request)
    {
        var result = await _agentService.UpdateAsync(agentNo, subIns, request);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpDelete("{agentNo}/{subIns}")]
    public async Task<IActionResult> Delete(string agentNo, string subIns)
    {
        var result = await _agentService.DeleteAsync(agentNo, subIns);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }
}
