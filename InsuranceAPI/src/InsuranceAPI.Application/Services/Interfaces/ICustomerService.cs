using InsuranceAPI.Application.DTOs.Admin;
using InsuranceAPI.Application.DTOs.Common;

namespace InsuranceAPI.Application.Services.Interfaces;

public interface ICustomerService
{
    Task<ApiResult<CustomerDto>> GetByIdAsync(long custNo);
    Task<ApiResult<PaginatedResult<CustomerDto>>> GetAllAsync(int page = 1, int pageSize = 20, string? search = null);
    Task<ApiResult<CustomerDto>> CreateAsync(CreateCustomerRequest request);
    Task<ApiResult<CustomerDto>> UpdateAsync(long custNo, UpdateCustomerRequest request);
    Task<ApiResult<bool>> DeleteAsync(long custNo);
}
