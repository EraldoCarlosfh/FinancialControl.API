using FinancialControl.Application.Dtos;
using FinancialControl.Application.Interfaces;
using FinancialControl.Application.Requests;
using FinancialControl.Domain.Dtos;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Enums;
using FinancialControl.Infra.Interfaces;

namespace FinancialControl.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<Transaction> AddTransactionAsync(TransactionRequest transactionRequest)
        {
            try
            {
                if (transactionRequest == null)
                    throw new ApplicationException("Dados para cadastro inválidos.");

                if (transactionRequest.Amount <= 0)
                    throw new ApplicationException("O valor da transação deve ser maior que zero.");

                var transaction = new Transaction();
                transaction.CreateTransaction(transactionRequest.Type, transactionRequest.Description, transactionRequest.Amount);

                _transactionRepository.AddAsync(transaction);
                await _transactionRepository.SaveChangesAsync();

                return transaction;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            var transactions = await _transactionRepository.GetAllAsync();
            if (transactions.Count() == 0)
                throw new ApplicationException($"Não existem transações cadastradas");

            return transactions;
        }

        public async Task<Transaction> GetByIdAsync(Guid id)
        {
            var existingTransaction = await _transactionRepository.GetByIdAsync(id);
            if (existingTransaction == null)
                throw new ApplicationException($"Não existe transação com Id: {id.ToString()} cadastrada");

            return existingTransaction;
        }

        public async Task<Transaction> UpdateTransactionAsync(TransactionRequest transactionDto)
        {     
            try
            {
                var existingTransaction = await _transactionRepository.GetByIdAsync(transactionDto.Id);
                if (existingTransaction == null)
                    throw new ApplicationException($"Não existe transação com Id: {transactionDto.Id.ToString()} cadastrada");

                existingTransaction.UpdateTransaction(transactionDto.Type, transactionDto.Description, transactionDto.Amount);

                _transactionRepository.UpdateAsync(existingTransaction);
                await _transactionRepository.SaveChangesAsync();

                return existingTransaction;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public async Task<decimal> GetDailyBalanceAsync(DateTime date)
        {
            if (date > DateTime.Today.AddDays(1))
                throw new ApplicationException($"Data não pode ser no futuro distante: {date}");

            try
            {
                var transactions = await _transactionRepository.GetByDateRangeAsync(
                    date.Date,
                    date.Date.AddDays(1).AddTicks(-1));

                return CalculateDailyBalance(transactions);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public async Task<IEnumerable<DailyBalanceResultDto>> GetNegativeBalanceDaysAsync()
        {
            return await _transactionRepository.GetNegativeBalanceDaysAsync();
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _transactionRepository.GetByDateRangeAsync(startDate, endDate);
        }

        private decimal CalculateDailyBalance(IEnumerable<Transaction> transactions)
        {
            try
            {
                return transactions.Sum(t =>
                    t.Type == TransactionType.Receita ? t.Amount : -t.Amount);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Falha ao calcular saldo diário");
            }
        }
    }
}
