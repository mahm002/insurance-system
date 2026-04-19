using InsuranceAPI.Application.DTOs.Claims;
using InsuranceAPI.Application.DTOs.Common;

namespace InsuranceAPI.Application.Services.Interfaces;

public interface IClaimService
{
    Task<ApiResult<ClaimDto>> GetByIdAsync(string clmNo);
    Task<ApiResult<PaginatedResult<ClaimDto>>> SearchAsync(ClaimSearchRequest request);
    Task<ApiResult<ClaimDto>> OpenClaimAsync(OpenClaimRequest request, string userName);
    Task<ApiResult<ClaimEstimationDto>> EstimateAsync(string clmNo, EstimateClaimRequest request, string userName);
    Task<ApiResult<ClaimSettlementDto>> SettleAsync(string clmNo, SettleClaimRequest request, string userName);
    Task<ApiResult<bool>> CloseAsync(string clmNo, CloseClaimRequest request, string userName);
    Task<ApiResult<bool>> ReopenAsync(string clmNo, string userName);
    Task<ApiResult<IEnumerable<ClaimEstimationDto>>> GetEstimationsAsync(string clmNo);
    Task<ApiResult<IEnumerable<ClaimSettlementDto>>> GetSettlementsAsync(string clmNo);
}
