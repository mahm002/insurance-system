using InsuranceAPI.Application.DTOs.Admin;
using InsuranceAPI.Application.DTOs.Common;

namespace InsuranceAPI.Application.Services.Interfaces;

public interface IAgentService
{
    Task<ApiResult<AgentCommissionDto>> GetByIdAsync(string agentNo, string subIns);
    Task<ApiResult<PaginatedResult<AgentCommissionDto>>> GetAllAsync(int page = 1, int pageSize = 20);
    Task<ApiResult<AgentCommissionDto>> CreateAsync(CreateAgentCommissionRequest request);
    Task<ApiResult<AgentCommissionDto>> UpdateAsync(string agentNo, string subIns, UpdateAgentCommissionRequest request);
    Task<ApiResult<bool>> DeleteAsync(string agentNo, string subIns);
}
