using InsuranceAPI.Application.DTOs.Admin;
using InsuranceAPI.Application.DTOs.Common;
using InsuranceAPI.Application.Services.Interfaces;
using InsuranceAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InsuranceAPI.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResult<UserDto>> GetByIdAsync(int accountNo)
    {
        var user = await _context.Users.FindAsync(accountNo);
        if (user == null)
            return ApiResult<UserDto>.Fail("User not found.");

        return ApiResult<UserDto>.Ok(MapToDto(user));
    }

    public async Task<ApiResult<PaginatedResult<UserDto>>> GetAllAsync(int page = 1, int pageSize = 20)
    {
        var query = _context.Users.AsQueryable();
        var totalCount = await query.CountAsync();

        var users = await query
            .OrderBy(u => u.AccountNo)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = new PaginatedResult<UserDto>
        {
            Items = users.Select(MapToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };

        return ApiResult<PaginatedResult<UserDto>>.Ok(result);
    }

    public async Task<ApiResult<UserDto>> CreateAsync(CreateUserRequest request)
    {
        var exists = await _context.Users.AnyAsync(u => u.AccountLogIn == request.AccountLogIn);
        if (exists)
            return ApiResult<UserDto>.Fail("Username already exists.");

        var user = new Domain.Entities.User
        {
            AccountLogIn = request.AccountLogIn,
            AccountName = request.AccountName,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Branch = request.Branch,
            AccountPermSys = request.AccountPermSys,
            AccountPermClm = request.AccountPermClm,
            AccountPermFin = request.AccountPermFin,
            AccountPermRe = request.AccountPermRe,
            AccountPermMan = request.AccountPermMan,
            AccountSysManag = request.AccountSysManag,
            AccLimit = request.AccLimit,
            IsActive = true
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return ApiResult<UserDto>.Ok(MapToDto(user), "User created successfully.");
    }

    public async Task<ApiResult<UserDto>> UpdateAsync(int accountNo, UpdateUserRequest request)
    {
        var user = await _context.Users.FindAsync(accountNo);
        if (user == null)
            return ApiResult<UserDto>.Fail("User not found.");

        user.AccountName = request.AccountName;
        user.Branch = request.Branch;
        user.AccountPermSys = request.AccountPermSys;
        user.AccountPermClm = request.AccountPermClm;
        user.AccountPermFin = request.AccountPermFin;
        user.AccountPermRe = request.AccountPermRe;
        user.AccountPermMan = request.AccountPermMan;
        user.AccountSysManag = request.AccountSysManag;
        user.AccLimit = request.AccLimit;
        user.IsActive = request.IsActive;

        await _context.SaveChangesAsync();

        return ApiResult<UserDto>.Ok(MapToDto(user), "User updated successfully.");
    }

    public async Task<ApiResult<bool>> DeleteAsync(int accountNo)
    {
        var user = await _context.Users.FindAsync(accountNo);
        if (user == null)
            return ApiResult<bool>.Fail("User not found.");

        user.IsActive = false;
        await _context.SaveChangesAsync();

        return ApiResult<bool>.Ok(true, "User deactivated successfully.");
    }

    public async Task<ApiResult<bool>> ResetPasswordAsync(int accountNo, string newPassword)
    {
        var user = await _context.Users.FindAsync(accountNo);
        if (user == null)
            return ApiResult<bool>.Fail("User not found.");

        user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
        await _context.SaveChangesAsync();

        return ApiResult<bool>.Ok(true, "Password reset successfully.");
    }

    private static UserDto MapToDto(Domain.Entities.User user) => new()
    {
        AccountNo = user.AccountNo,
        AccountLogIn = user.AccountLogIn,
        AccountName = user.AccountName,
        Branch = user.Branch,
        AccountPermSys = user.AccountPermSys,
        AccountPermClm = user.AccountPermClm,
        AccountPermFin = user.AccountPermFin,
        AccountPermRe = user.AccountPermRe,
        AccountPermMan = user.AccountPermMan,
        AccountSysManag = user.AccountSysManag,
        AccLimit = user.AccLimit,
        IsActive = user.IsActive
    };
}
