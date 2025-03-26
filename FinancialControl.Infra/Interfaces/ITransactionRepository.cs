using FinancialControl.Domain.Entities;

namespace FinancialControl.Infra.Interfaces
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction transaction);
        Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetDailyBalanceAsync(DateTime date);
    }
}
