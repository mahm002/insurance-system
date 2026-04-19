using InsuranceAPI.Application.DTOs.Admin;
using InsuranceAPI.Application.DTOs.Common;
using InsuranceAPI.Application.Services.Interfaces;
using InsuranceAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InsuranceAPI.Infrastructure.Services;

public class AgentService : IAgentService
{
    private readonly AppDbContext _context;

    public AgentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResult<AgentDto>> GetByIdAsync(int agentNo)
    {
        var agent = await _context.Agents.FindAsync(agentNo);
        if (agent == null)
            return ApiResult<AgentDto>.Fail("Agent not found.");

        return ApiResult<AgentDto>.Ok(MapToDto(agent));
    }

    public async Task<ApiResult<PaginatedResult<AgentDto>>> GetAllAsync(int page = 1, int pageSize = 20)
    {
        var query = _context.Agents.AsQueryable();
        var totalCount = await query.CountAsync();

        var agents = await query
            .OrderBy(a => a.AgentNo)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = new PaginatedResult<AgentDto>
        {
            Items = agents.Select(MapToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };

        return ApiResult<PaginatedResult<AgentDto>>.Ok(result);
    }

    public async Task<ApiResult<AgentDto>> CreateAsync(CreateAgentRequest request)
    {
        var agent = new Domain.Entities.Agent
        {
            AgentName = request.AgentName,
            AgentNameE = request.AgentNameE,
            Address = request.Address,
            TelNo = request.TelNo,
            Email = request.Email,
            Branch = request.Branch,
            CommissionRate = request.CommissionRate,
            IsActive = true
        };

        _context.Agents.Add(agent);
        await _context.SaveChangesAsync();

        return ApiResult<AgentDto>.Ok(MapToDto(agent), "Agent created successfully.");
    }

    public async Task<ApiResult<AgentDto>> UpdateAsync(int agentNo, UpdateAgentRequest request)
    {
        var agent = await _context.Agents.FindAsync(agentNo);
        if (agent == null)
            return ApiResult<AgentDto>.Fail("Agent not found.");

        agent.AgentName = request.AgentName;
        agent.AgentNameE = request.AgentNameE;
        agent.Address = request.Address;
        agent.TelNo = request.TelNo;
        agent.Email = request.Email;
        agent.Branch = request.Branch;
        agent.CommissionRate = request.CommissionRate;
        agent.IsActive = request.IsActive;

        await _context.SaveChangesAsync();

        return ApiResult<AgentDto>.Ok(MapToDto(agent), "Agent updated successfully.");
    }

    public async Task<ApiResult<bool>> DeleteAsync(int agentNo)
    {
        var agent = await _context.Agents.FindAsync(agentNo);
        if (agent == null)
            return ApiResult<bool>.Fail("Agent not found.");

        agent.IsActive = false;
        await _context.SaveChangesAsync();

        return ApiResult<bool>.Ok(true, "Agent deactivated successfully.");
    }

    private static AgentDto MapToDto(Domain.Entities.Agent a) => new()
    {
        AgentNo = a.AgentNo,
        AgentName = a.AgentName,
        AgentNameE = a.AgentNameE,
        Address = a.Address,
        TelNo = a.TelNo,
        Email = a.Email,
        Branch = a.Branch,
        CommissionRate = a.CommissionRate,
        AccNo = a.AccNo,
        IsActive = a.IsActive
    };
}
