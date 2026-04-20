using InsuranceAPI.Application.DTOs.Admin;
using InsuranceAPI.Application.DTOs.Common;
using InsuranceAPI.Application.Services.Interfaces;
using InsuranceAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InsuranceAPI.Infrastructure.Services;

public class CustomerService : ICustomerService
{
    private readonly AppDbContext _context;

    public CustomerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResult<CustomerDto>> GetByIdAsync(long custNo)
    {
        var customer = await _context.Customers.FindAsync(custNo);
        if (customer == null)
            return ApiResult<CustomerDto>.Fail("Customer not found.");

        return ApiResult<CustomerDto>.Ok(MapToDto(customer));
    }

    public async Task<ApiResult<PaginatedResult<CustomerDto>>> GetAllAsync(int page = 1, int pageSize = 20, string? search = null)
    {
        var query = _context.Customers.AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(c =>
                c.CustName.Contains(search) ||
                c.CustNameE.Contains(search) ||
                c.IDNo.Contains(search) ||
                c.TelNo.Contains(search));
        }

        var totalCount = await query.CountAsync();

        var customers = await query
            .OrderBy(c => c.CustNo)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = new PaginatedResult<CustomerDto>
        {
            Items = customers.Select(MapToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };

        return ApiResult<PaginatedResult<CustomerDto>>.Ok(result);
    }

    public async Task<ApiResult<CustomerDto>> CreateAsync(CreateCustomerRequest request)
    {
        var customer = new Domain.Entities.Customer
        {
            CustNo = request.CustNo,
            CustTP = request.CustTP,
            CustName = request.CustName,
            CustNameE = request.CustNameE,
            IDNo = request.IDNo,
            DrCardNo = request.DrCardNo,
            TelNo = request.TelNo,
            Address = request.Address,
            Email = request.Email,
            SpecialCase = request.SpecialCase,
            RecDate = DateTime.UtcNow,
            FaxNo = string.Empty
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return ApiResult<CustomerDto>.Ok(MapToDto(customer), "Customer created successfully.");
    }

    public async Task<ApiResult<CustomerDto>> UpdateAsync(long custNo, UpdateCustomerRequest request)
    {
        var customer = await _context.Customers.FindAsync(custNo);
        if (customer == null)
            return ApiResult<CustomerDto>.Fail("Customer not found.");

        customer.CustName = request.CustName;
        customer.CustNameE = request.CustNameE;
        customer.IDNo = request.IDNo;
        customer.DrCardNo = request.DrCardNo;
        customer.TelNo = request.TelNo;
        customer.Address = request.Address;
        customer.Email = request.Email;
        customer.SpecialCase = request.SpecialCase;

        await _context.SaveChangesAsync();

        return ApiResult<CustomerDto>.Ok(MapToDto(customer), "Customer updated successfully.");
    }

    public async Task<ApiResult<bool>> DeleteAsync(long custNo)
    {
        var customer = await _context.Customers.FindAsync(custNo);
        if (customer == null)
            return ApiResult<bool>.Fail("Customer not found.");

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();

        return ApiResult<bool>.Ok(true, "Customer deleted successfully.");
    }

    private static CustomerDto MapToDto(Domain.Entities.Customer c) => new()
    {
        CustNo = c.CustNo,
        CustTP = c.CustTP,
        CustName = c.CustName,
        CustNameE = c.CustNameE,
        IDNo = c.IDNo,
        DrCardNo = c.DrCardNo,
        TelNo = c.TelNo,
        Address = c.Address,
        Email = c.Email,
        SpecialCase = c.SpecialCase,
        AccNo = c.AccNo
    };
}
