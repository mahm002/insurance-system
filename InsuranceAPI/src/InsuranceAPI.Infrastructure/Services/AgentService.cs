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

    public async Task<ApiResult<AgentCommissionDto>> GetByIdAsync(string agentNo, string subIns)
    {
        var agent = await _context.AgentCommissions.FindAsync(agentNo, subIns);
        if (agent == null)
            return ApiResult<AgentCommissionDto>.Fail("Agent commission not found.");

        return ApiResult<AgentCommissionDto>.Ok(MapToDto(agent));
    }

    public async Task<ApiResult<PaginatedResult<AgentCommissionDto>>> GetAllAsync(int page = 1, int pageSize = 20)
    {
        var query = _context.AgentCommissions.AsQueryable();
        var totalCount = await query.CountAsync();

        var agents = await query
            .OrderBy(a => a.AgentNo)
            .ThenBy(a => a.SubIns)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = new PaginatedResult<AgentCommissionDto>
        {
            Items = agents.Select(MapToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };

        return ApiResult<PaginatedResult<AgentCommissionDto>>.Ok(result);
    }

    public async Task<ApiResult<AgentCommissionDto>> CreateAsync(CreateAgentCommissionRequest request)
    {
        var exists = await _context.AgentCommissions
            .AnyAsync(a => a.AgentNo == request.AgentNo && a.SubIns == request.SubIns);
        if (exists)
            return ApiResult<AgentCommissionDto>.Fail("Agent commission for this SubIns already exists.");

        var agent = new Domain.Entities.AgentCommission
        {
            AgentNo = request.AgentNo,
            SubIns = request.SubIns,
            Comm = request.Comm,
            AccountNo = request.AccountNo
        };

        _context.AgentCommissions.Add(agent);
        await _context.SaveChangesAsync();

        return ApiResult<AgentCommissionDto>.Ok(MapToDto(agent), "Agent commission created successfully.");
    }

    public async Task<ApiResult<AgentCommissionDto>> UpdateAsync(string agentNo, string subIns, UpdateAgentCommissionRequest request)
    {
        var agent = await _context.AgentCommissions.FindAsync(agentNo, subIns);
        if (agent == null)
            return ApiResult<AgentCommissionDto>.Fail("Agent commission not found.");

        agent.Comm = request.Comm;
        agent.AccountNo = request.AccountNo;

        await _context.SaveChangesAsync();

        return ApiResult<AgentCommissionDto>.Ok(MapToDto(agent), "Agent commission updated successfully.");
    }

    public async Task<ApiResult<bool>> DeleteAsync(string agentNo, string subIns)
    {
        var agent = await _context.AgentCommissions.FindAsync(agentNo, subIns);
        if (agent == null)
            return ApiResult<bool>.Fail("Agent commission not found.");

        _context.AgentCommissions.Remove(agent);
        await _context.SaveChangesAsync();

        return ApiResult<bool>.Ok(true, "Agent commission deleted successfully.");
    }

    private static AgentCommissionDto MapToDto(Domain.Entities.AgentCommission a) => new()
    {
        Id = a.Id,
        AgentNo = a.AgentNo,
        SubIns = a.SubIns,
        Comm = a.Comm,
        AccountNo = a.AccountNo
    };
}
