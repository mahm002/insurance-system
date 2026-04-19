using InsuranceAPI.Application.DTOs.Admin;
using InsuranceAPI.Application.DTOs.Common;

namespace InsuranceAPI.Application.Services.Interfaces;

public interface IBranchService
{
    Task<ApiResult<BranchDto>> GetByIdAsync(string branchNo);
    Task<ApiResult<PaginatedResult<BranchDto>>> GetAllAsync(int page = 1, int pageSize = 20);
    Task<ApiResult<BranchDto>> CreateAsync(CreateBranchRequest request);
    Task<ApiResult<BranchDto>> UpdateAsync(string branchNo, UpdateBranchRequest request);
    Task<ApiResult<bool>> DeleteAsync(string branchNo);
}
