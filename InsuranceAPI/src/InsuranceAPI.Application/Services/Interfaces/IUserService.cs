using InsuranceAPI.Application.DTOs.Admin;
using InsuranceAPI.Application.DTOs.Common;

namespace InsuranceAPI.Application.Services.Interfaces;

public interface IUserService
{
    Task<ApiResult<UserDto>> GetByIdAsync(string accountLogIn);
    Task<ApiResult<PaginatedResult<UserDto>>> GetAllAsync(int page = 1, int pageSize = 20);
    Task<ApiResult<UserDto>> CreateAsync(CreateUserRequest request);
    Task<ApiResult<UserDto>> UpdateAsync(string accountLogIn, UpdateUserRequest request);
    Task<ApiResult<bool>> DeleteAsync(string accountLogIn);
    Task<ApiResult<bool>> ResetPasswordAsync(string accountLogIn, string newPassword);
}
