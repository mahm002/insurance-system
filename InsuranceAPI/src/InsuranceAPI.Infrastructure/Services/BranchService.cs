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
            BranchCode = request.BranchCode,
            BranchName = request.BranchName,
            BranchNameEn = request.BranchNameEn,
            Address = request.Address,
            Telephone = request.Telephone,
            FaxNo = request.FaxNo,
            EMail = request.EMail,
            ManagerId = request.ManagerId,
            AccountingCode = request.AccountingCode,
            SystemURI = string.Empty,
            CashierAccount = string.Empty,
            ChequeAccount = string.Empty
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
        branch.BranchNameEn = request.BranchNameEn;
        branch.Address = request.Address;
        branch.Telephone = request.Telephone;
        branch.FaxNo = request.FaxNo;
        branch.EMail = request.EMail;
        branch.ManagerId = request.ManagerId;
        branch.CashierAccount = request.CashierAccount ?? branch.CashierAccount;
        branch.ChequeAccount = request.ChequeAccount ?? branch.ChequeAccount;
        branch.AccountingCode = request.AccountingCode;

        await _context.SaveChangesAsync();

        return ApiResult<BranchDto>.Ok(MapToDto(branch), "Branch updated successfully.");
    }

    public async Task<ApiResult<bool>> DeleteAsync(string branchNo)
    {
        var branch = await _context.Branches.FindAsync(branchNo);
        if (branch == null)
            return ApiResult<bool>.Fail("Branch not found.");

        _context.Branches.Remove(branch);
        await _context.SaveChangesAsync();

        return ApiResult<bool>.Ok(true, "Branch deleted successfully.");
    }

    private static BranchDto MapToDto(Domain.Entities.Branch branch) => new()
    {
        BranchNo = branch.BranchNo,
        BranchCode = branch.BranchCode,
        Agent = branch.Agent,
        BranchName = branch.BranchName,
        BranchNameEn = branch.BranchNameEn,
        Address = branch.Address,
        Telephone = branch.Telephone,
        FaxNo = branch.FaxNo,
        EMail = branch.EMail,
        ManagerId = branch.ManagerId,
        CashierAccount = branch.CashierAccount,
        ChequeAccount = branch.ChequeAccount,
        AccountingCode = branch.AccountingCode,
        MainCenter = branch.MainCenter,
        CompanyOffice = branch.CompanyOffice
    };
}
