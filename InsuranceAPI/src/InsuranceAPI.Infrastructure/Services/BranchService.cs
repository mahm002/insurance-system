using InsuranceAPI.Application.DTOs.Admin;
using InsuranceAPI.Application.DTOs.Common;
using InsuranceAPI.Application.Services.Interfaces;
using InsuranceAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InsuranceAPI.Infrastructure.Services;

public class BranchService : IBranchService
{
    private readonly AppDbContext _context;

    public BranchService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResult<BranchDto>> GetByIdAsync(string branchNo)
    {
        var branch = await _context.Branches.FindAsync(branchNo);
        if (branch == null)
            return ApiResult<BranchDto>.Fail("Branch not found.");

        return ApiResult<BranchDto>.Ok(MapToDto(branch));
    }

    public async Task<ApiResult<PaginatedResult<BranchDto>>> GetAllAsync(int page = 1, int pageSize = 20)
    {
        var query = _context.Branches.AsQueryable();
        var totalCount = await query.CountAsync();

        var branches = await query
            .OrderBy(b => b.BranchNo)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = new PaginatedResult<BranchDto>
        {
            Items = branches.Select(MapToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };

        return ApiResult<PaginatedResult<BranchDto>>.Ok(result);
    }

    public async Task<ApiResult<BranchDto>> CreateAsync(CreateBranchRequest request)
    {
        var exists = await _context.Branches.AnyAsync(b => b.BranchNo == request.BranchNo);
        if (exists)
            return ApiResult<BranchDto>.Fail("Branch number already exists.");

        var branch = new Domain.Entities.Branch
        {
            BranchNo = request.BranchNo,
            BranchName = request.BranchName,
            BranchNameE = request.BranchNameE,
            Address = request.Address,
            TelNo = request.TelNo,
            FaxNo = request.FaxNo,
            Email = request.Email,
            ManagerId = request.ManagerId,
            IsActive = true
        };

        _context.Branches.Add(branch);
        await _context.SaveChangesAsync();

        return ApiResult<BranchDto>.Ok(MapToDto(branch), "Branch created successfully.");
    }

    public async Task<ApiResult<BranchDto>> UpdateAsync(string branchNo, UpdateBranchRequest request)
    {
        var branch = await _context.Branches.FindAsync(branchNo);
        if (branch == null)
            return ApiResult<BranchDto>.Fail("Branch not found.");

        branch.BranchName = request.BranchName;
        branch.BranchNameE = request.BranchNameE;
        branch.Address = request.Address;
        branch.TelNo = request.TelNo;
        branch.FaxNo = request.FaxNo;
        branch.Email = request.Email;
        branch.ManagerId = request.ManagerId;
        branch.CashierAccount = request.CashierAccount;
        branch.ChequeAccount = request.ChequeAccount;
        branch.IsActive = request.IsActive;

        await _context.SaveChangesAsync();

        return ApiResult<BranchDto>.Ok(MapToDto(branch), "Branch updated successfully.");
    }

    public async Task<ApiResult<bool>> DeleteAsync(string branchNo)
    {
        var branch = await _context.Branches.FindAsync(branchNo);
        if (branch == null)
            return ApiResult<bool>.Fail("Branch not found.");

        branch.IsActive = false;
        await _context.SaveChangesAsync();

        return ApiResult<bool>.Ok(true, "Branch deactivated successfully.");
    }

    private static BranchDto MapToDto(Domain.Entities.Branch branch) => new()
    {
        BranchNo = branch.BranchNo,
        BranchName = branch.BranchName,
        BranchNameE = branch.BranchNameE,
        Address = branch.Address,
        TelNo = branch.TelNo,
        FaxNo = branch.FaxNo,
        Email = branch.Email,
        ManagerId = branch.ManagerId,
        CashierAccount = branch.CashierAccount,
        ChequeAccount = branch.ChequeAccount,
        AccountingCode = branch.AccountingCode,
        IsActive = branch.IsActive
    };
}
