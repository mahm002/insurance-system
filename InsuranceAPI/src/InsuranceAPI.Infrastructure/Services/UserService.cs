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

    public async Task<ApiResult<UserDto>> GetByIdAsync(string accountLogIn)
    {
        var user = await _context.Users.FindAsync(accountLogIn);
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
            AccountPassWord = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Branch = request.Branch,
            AccountPermSys = request.AccountPermSys,
            AccountPermClm = request.AccountPermClm,
            AccountPermFin = request.AccountPermFin,
            AccountPermRe = request.AccountPermRe,
            AccountPermMan = request.AccountPermMan,
            AccountSysManag = request.AccountSysManag,
            AccLimit = request.AccLimit,
            AddedBy = request.AccountLogIn,
            ModifiedBy = request.AccountLogIn,
            ModifyDate = DateTime.UtcNow,
            Stop = false
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return ApiResult<UserDto>.Ok(MapToDto(user), "User created successfully.");
    }

    public async Task<ApiResult<UserDto>> UpdateAsync(string accountLogIn, UpdateUserRequest request)
    {
        var user = await _context.Users.FindAsync(accountLogIn);
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
        user.Stop = request.Stop;
        user.ModifiedBy = accountLogIn;
        user.ModifyDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return ApiResult<UserDto>.Ok(MapToDto(user), "User updated successfully.");
    }

    public async Task<ApiResult<bool>> DeleteAsync(string accountLogIn)
    {
        var user = await _context.Users.FindAsync(accountLogIn);
        if (user == null)
            return ApiResult<bool>.Fail("User not found.");

        user.Stop = true;
        user.ModifiedBy = accountLogIn;
        user.ModifyDate = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return ApiResult<bool>.Ok(true, "User deactivated successfully.");
    }

    public async Task<ApiResult<bool>> ResetPasswordAsync(string accountLogIn, string newPassword)
    {
        var user = await _context.Users.FindAsync(accountLogIn);
        if (user == null)
            return ApiResult<bool>.Fail("User not found.");

        user.AccountPassWord = BCrypt.Net.BCrypt.HashPassword(newPassword);
        user.ModifiedBy = accountLogIn;
        user.ModifyDate = DateTime.UtcNow;
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
        Stop = user.Stop
    };
}
