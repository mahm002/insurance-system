using InsuranceAPI.Application.DTOs.Common;
using InsuranceAPI.Application.DTOs.Finance;

namespace InsuranceAPI.Application.Services.Interfaces;

public interface IFinanceService
{
    // Accounts
    Task<ApiResult<AccountDto>> GetAccountAsync(string accountNo);
    Task<ApiResult<IEnumerable<AccountDto>>> GetAccountTreeAsync(string? parentAcc = null);
    Task<ApiResult<AccountDto>> CreateAccountAsync(CreateAccountRequest request);

    // Receipts
    Task<ApiResult<ReceiptDto>> GetReceiptAsync(string docNo);
    Task<ApiResult<PaginatedResult<ReceiptDto>>> GetReceiptsAsync(int page = 1, int pageSize = 20, string? branch = null);
    Task<ApiResult<ReceiptDto>> CreateReceiptAsync(CreateReceiptRequest request, string userName);

    // Multi-Payment
    Task<ApiResult<MultiPaymentResponse>> ProcessMultiPaymentAsync(MultiPaymentRequest request, string userName);

    // Journal
    Task<ApiResult<JournalEntryDto>> GetJournalAsync(string dailyNum);
    Task<ApiResult<PaginatedResult<JournalEntryDto>>> GetJournalsAsync(int page = 1, int pageSize = 20, string? branch = null);
    Task<ApiResult<JournalEntryDto>> CreateJournalAsync(CreateJournalRequest request, string userName);
}
