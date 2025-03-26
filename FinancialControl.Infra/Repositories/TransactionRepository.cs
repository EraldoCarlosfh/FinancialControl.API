using FinancialControl.Domain.Dtos;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Enums;
using FinancialControl.Infra.Data;
using FinancialControl.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Infra.Repositories
{
    public class TransactionRepository : GeralRepo, ITransactionRepository
    {
        private readonly DataContext _context;

        public TransactionRepository(DataContext context) : base(context)
        {
            _context = context;
        }
       
        public async Task<Transaction> GetByIdAsync(Guid id)
        {
            return await _context.Transactions
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Transactions
                .Where(t => t.Date >= startDate && t.Date <= endDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return await _context.Transactions
                .AsNoTracking()
                .ToListAsync();
        }        

        public async Task<DailyBalanceResultDto> GetDailyBalanceOptimizedAsync(DateTime date)
        {
            var startDate = date.Date;
            var endDate = startDate.AddDays(1).AddTicks(-1);

            var result = await _context.Transactions
                .Where(t => t.Date >= startDate && t.Date <= endDate)
                .GroupBy(t => 1)
                .Select(g => new DailyBalanceResultDto
                {
                    Date = startDate,
                    TotalIncome = g.Where(t => t.Type == TransactionType.Receita).Sum(t => t.Amount),
                    TotalExpenses = g.Where(t => t.Type == TransactionType.Despesa).Sum(t => t.Amount),
                    Balance = g.Sum(t => t.Type == TransactionType.Receita ? t.Amount : -t.Amount)
                })
                .FirstOrDefaultAsync();

            return result ?? new DailyBalanceResultDto { Date = date.Date };
        }

        public async Task<decimal> GetDailyBalanceAsync(DateTime date)
        {
            var startDate = date.Date;
            var endDate = startDate.AddDays(1).AddTicks(-1);

            return await _context.Transactions
                .Where(t => t.Date >= startDate && t.Date <= endDate)
                .SumAsync(t => t.Type == TransactionType.Receita ? t.Amount : -t.Amount);
        }

        public async Task<IEnumerable<DailyBalanceResultDto>> GetNegativeBalanceDaysAsync()
        {
            var dailyBalances = await _context.Transactions
                .AsNoTracking()
                .GroupBy(t => t.Date.Date)
                .Select(g => new DailyBalanceResultDto
                {
                    Date = g.Key,
                    TotalIncome = g.Where(t => t.Type == TransactionType.Receita).Sum(t => t.Amount),
                    TotalExpenses = g.Where(t => t.Type == TransactionType.Despesa).Sum(t => t.Amount),
                    Balance = g.Sum(t => t.Type == TransactionType.Receita ? t.Amount : -t.Amount)
                })
                .Where(d => d.Balance < 0)
                .OrderBy(d => d.Date)
                .ToListAsync();

            return dailyBalances;
        }
    }
}
