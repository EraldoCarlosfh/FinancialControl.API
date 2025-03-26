using FinancialControl.Application.Dtos;
using FinancialControl.Application.Requests;
using FinancialControl.Domain.Dtos;
using FinancialControl.Domain.Entities;

namespace FinancialControl.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<Transaction> AddTransactionAsync(TransactionDto transactionDto);
        Task<Transaction> GetByIdAsync(Guid id);
        Task<Transaction> UpdateTransactionAsync(TransactionRequest transactionDto);
        Task<decimal> GetDailyBalanceAsync(DateTime date);
        Task<IEnumerable<DailyBalanceResultDto>> GetNegativeBalanceDaysAsync();
        Task<IEnumerable<Transaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
