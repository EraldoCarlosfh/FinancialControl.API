using FinancialControl.Domain.Dtos;
using FinancialControl.Domain.Entities;

namespace FinancialControl.Infra.Interfaces
{
    public interface ITransactionRepository : IGeralRepo
    {
        Task<Transaction> GetByIdAsync(Guid id);
        Task<IEnumerable<Transaction>> GetAllAsync();
        Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetDailyBalanceAsync(DateTime date);
        Task<DailyBalanceResultDto> GetDailyBalanceOptimizedAsync(DateTime date);
        Task<IEnumerable<DailyBalanceResultDto>> GetNegativeBalanceDaysAsync();
    }
}
