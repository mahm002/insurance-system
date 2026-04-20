using InsuranceAPI.Application.DTOs.Auth;
using InsuranceAPI.Application.DTOs.Common;

namespace InsuranceAPI.Application.Services.Interfaces;

public interface IAuthService
{
    Task<ApiResult<LoginResponse>> LoginAsync(LoginRequest request);
    Task<ApiResult<LoginResponse>> RefreshTokenAsync(RefreshTokenRequest request);
    Task<ApiResult<bool>> ChangePasswordAsync(string accountLogIn, ChangePasswordRequest request);
}
