using InsuranceAPI.Application.DTOs.Admin;
using InsuranceAPI.Application.DTOs.Common;

namespace InsuranceAPI.Application.Services.Interfaces;

public interface IAgentService
{
    Task<ApiResult<AgentDto>> GetByIdAsync(int agentNo);
    Task<ApiResult<PaginatedResult<AgentDto>>> GetAllAsync(int page = 1, int pageSize = 20);
    Task<ApiResult<AgentDto>> CreateAsync(CreateAgentRequest request);
    Task<ApiResult<AgentDto>> UpdateAsync(int agentNo, UpdateAgentRequest request);
    Task<ApiResult<bool>> DeleteAsync(int agentNo);
}
