using InsuranceAPI.Application.DTOs.Admin;
using InsuranceAPI.Application.DTOs.Common;

namespace InsuranceAPI.Application.Services.Interfaces;

public interface IUserService
{
    Task<ApiResult<UserDto>> GetByIdAsync(int accountNo);
    Task<ApiResult<PaginatedResult<UserDto>>> GetAllAsync(int page = 1, int pageSize = 20);
    Task<ApiResult<UserDto>> CreateAsync(CreateUserRequest request);
    Task<ApiResult<UserDto>> UpdateAsync(int accountNo, UpdateUserRequest request);
    Task<ApiResult<bool>> DeleteAsync(int accountNo);
    Task<ApiResult<bool>> ResetPasswordAsync(int accountNo, string newPassword);
}
