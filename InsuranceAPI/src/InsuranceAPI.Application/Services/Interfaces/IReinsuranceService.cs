using InsuranceAPI.Application.DTOs.Common;
using InsuranceAPI.Application.DTOs.Reinsurance;

namespace InsuranceAPI.Application.Services.Interfaces;

public interface IReinsuranceService
{
    Task<ApiResult<TreatyDto>> GetTreatyAsync(int id);
    Task<ApiResult<PaginatedResult<TreatyDto>>> GetTreatiesAsync(int page = 1, int pageSize = 20, string? subIns = null);
    Task<ApiResult<TreatyDto>> CreateTreatyAsync(CreateTreatyRequest request);
    Task<ApiResult<TreatyDto>> UpdateTreatyAsync(int id, UpdateTreatyRequest request);
    Task<ApiResult<bool>> DeleteTreatyAsync(int id);
    Task<ApiResult<bool>> DistributePolicyAsync(DistributePolicyRequest request);
    Task<ApiResult<bool>> DistributeClaimAsync(DistributeClaimRequest request);
    Task<ApiResult<IEnumerable<RiskProfileDto>>> GetRiskProfileAsync(string? branch = null);
}
