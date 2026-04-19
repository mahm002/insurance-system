using InsuranceAPI.Application.DTOs.Common;
using InsuranceAPI.Application.DTOs.Policies;

namespace InsuranceAPI.Application.Services.Interfaces;

public interface IPolicyService
{
    Task<ApiResult<PolicyDto>> GetByIdAsync(string orderNo, int endNo, int loadNo, string subIns);
    Task<ApiResult<PaginatedResult<PolicyDto>>> SearchAsync(PolicySearchRequest request);
    Task<ApiResult<PolicyDto>> CreateAsync(CreatePolicyRequest request, string userName);
    Task<ApiResult<PolicyDto>> UpdateAsync(string orderNo, int endNo, int loadNo, string subIns, UpdatePolicyRequest request);
    Task<ApiResult<PolicyDto>> EndorseAsync(EndorsePolicyRequest request, string userName);
    Task<ApiResult<bool>> IssueAsync(IssuePolicyRequest request, string userName);
    Task<ApiResult<bool>> CancelAsync(CancelPolicyRequest request, string userName);
}
