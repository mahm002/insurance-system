using InsuranceAPI.Application.DTOs.Admin;
using InsuranceAPI.Application.DTOs.Common;

namespace InsuranceAPI.Application.Services.Interfaces;

public interface ICustomerService
{
    Task<ApiResult<CustomerDto>> GetByIdAsync(int custNo);
    Task<ApiResult<PaginatedResult<CustomerDto>>> GetAllAsync(int page = 1, int pageSize = 20, string? search = null);
    Task<ApiResult<CustomerDto>> CreateAsync(CreateCustomerRequest request);
    Task<ApiResult<CustomerDto>> UpdateAsync(int custNo, UpdateCustomerRequest request);
    Task<ApiResult<bool>> DeleteAsync(int custNo);
}
