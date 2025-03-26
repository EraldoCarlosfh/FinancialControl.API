using FinancialControl.Domain.Entities;
using FinancialControl.Infra.Data;
using FinancialControl.Infra.Interfaces;

namespace FinancialControl.Infra.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly DataContext _context;

        public TransactionRepository(DataContext context)
        {
            _context = context;
        }

        Task ITransactionRepository.AddAsync(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Transaction>> ITransactionRepository.GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        Task<decimal> ITransactionRepository.GetDailyBalanceAsync(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
