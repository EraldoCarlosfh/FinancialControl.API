using Xunit;
using Moq;
using FinancialControl.Application.Services;
using FinancialControl.Application.Dtos;
using FinancialControl.Application.Requests;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Enums;
using FinancialControl.Infra.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;

namespace FinancialControl.Tests.Services
{
    public class TransactionServiceTests
    {
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly TransactionService _transactionService;

        public TransactionServiceTests()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _transactionService = new TransactionService(_transactionRepositoryMock.Object);
        }

        [Fact]
        public async Task AddTransactionAsync_ShouldThrowException_WhenTransactionIsNull()
        {      
            var exception = await Assert.ThrowsAsync<ApplicationException>(() =>
                _transactionService.AddTransactionAsync(null));

            Assert.Equal("Dados para cadastro inválidos.", exception.Message);
        }

        [Fact]
        public async Task AddTransactionAsync_ShouldThrowException_WhenAmountIsZeroOrNegative()
        {
            var transactionDto = new TransactionRequest
            {
                Amount = 0,
                Description = "Test Transaction",
                Type = TransactionType.Receita
            };

            var exception = await Assert.ThrowsAsync<ApplicationException>(() =>
                _transactionService.AddTransactionAsync(transactionDto));

            Assert.Equal("O valor da transação deve ser maior que zero.", exception.Message);
        }

        [Fact]
        public async Task AddTransactionAsync_ShouldAddTransaction_WhenValidDataProvided()
        {    
            var transactionDto = new TransactionRequest
            {        
                Amount = 100,
                Description = "Test Transaction",
                Type = TransactionType.Receita
            };

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                Amount = transactionDto.Amount,
                Description = transactionDto.Description,
                Type = transactionDto.Type,
                Date = DateTime.UtcNow
            };

            _transactionRepositoryMock.Setup(repo => repo.AddAsync(transaction));
            _transactionRepositoryMock.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _transactionService.AddTransactionAsync(transactionDto);

            Assert.NotNull(result);           
            Assert.Equal(transactionDto.Amount, result.Amount);
            Assert.Equal(transactionDto.Description, result.Description);
            Assert.Equal(transactionDto.Type, result.Type);

            _transactionRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Transaction>()), Times.Once);
            _transactionRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GeAllAsync_ShouldReturnTransaction_WhenTransactionExists()
        {
            var expectedTransactions = new List<Transaction>
            {
                new Transaction { Id = Guid.NewGuid(), Type = TransactionType.Receita, Date = DateTime.Now, Amount = 100, Description = "Test 1" },
                new Transaction { Id = Guid.NewGuid(), Type = TransactionType.Despesa, Date = DateTime.Now, Amount = 200, Description = "Test 2" }
            };

            _transactionRepositoryMock.Setup(repo => repo.GetAllAsync())
                     .ReturnsAsync(expectedTransactions);

            var result = await _transactionService.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(expectedTransactions, result);
            _transactionRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldThrowException_WhenTransactionDoesNotExist()
        {
            var emptyTransactionList = Enumerable.Empty<Transaction>();

            _transactionRepositoryMock.Setup(repo => repo.GetAllAsync())
                     .ReturnsAsync(emptyTransactionList);

            var exception = await Assert.ThrowsAsync<ApplicationException>(
                () => _transactionService.GetAllAsync());

            Assert.Equal("Não existem transações cadastradas", exception.Message);
            _transactionRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnTransaction_WhenTransactionExists()
        {
            var transactionId = Guid.NewGuid();
            var transaction = new Transaction { Id = transactionId, Amount = 100 };

            _transactionRepositoryMock.Setup(repo => repo.GetByIdAsync(transactionId))
                .ReturnsAsync(transaction);

            var result = await _transactionService.GetByIdAsync(transactionId);

            Assert.NotNull(result);
            Assert.Equal(transactionId, result.Id);
            Assert.Equal(100, result.Amount);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowException_WhenTransactionDoesNotExist()
        {
            var transactionId = Guid.NewGuid();

            var exception = await Assert.ThrowsAsync<ApplicationException>(() =>
                _transactionService.GetByIdAsync(transactionId));

            Assert.Equal($"Não existe transação com Id: {transactionId.ToString()} cadastrada", exception.Message);
        }

        [Fact]
        public async Task UpdateTransactionAsync_ShouldThrowException_WhenTransactionIsNull()
        {
            var transactionRequest = new TransactionRequest
            {
                Amount = 100,
                Description = "Test Transaction",
                Type = TransactionType.Receita
            };           

            await Assert.ThrowsAsync<ApplicationException>(() =>
                _transactionService.UpdateTransactionAsync(transactionRequest));
        }

        [Fact]
        public async Task UpdateTransactionAsync_ShouldThrowException_WhenTransactionDoesNotExist()
        {
            var transactionId = Guid.NewGuid();

            var transactionRequest = new TransactionRequest
            {
                Id = Guid.NewGuid(),
                Amount = 100,
                Description = "Test Transaction",
                Type = TransactionType.Receita
            };

            var exception = await Assert.ThrowsAsync<ApplicationException>(() =>
                _transactionService.UpdateTransactionAsync(transactionRequest));

            Assert.Equal($"Não existe transação com Id: {transactionRequest.Id.ToString()} cadastrada", exception.Message);
        }

        [Fact]
        public async Task GetDailyBalanceAsync_ShouldThrowException_WhenDateIsInDistantFuture()
        {
            var futureDate = DateTime.Today.AddDays(2);

            var exception = await Assert.ThrowsAsync<ApplicationException>(() =>
            _transactionService.GetDailyBalanceAsync(futureDate));

            Assert.Equal($"Data não pode ser no futuro distante: {futureDate}", exception.Message);
        }

        [Fact]
        public async Task GetDailyBalanceAsync_ShouldReturnCorrectBalance()
        {
            var transactions = new List<Transaction>
            {
                new Transaction { Amount = 100, Type = TransactionType.Receita },
                new Transaction { Amount = 50, Type = TransactionType.Despesa }
            };

            _transactionRepositoryMock.Setup(repo => repo.GetByDateRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(transactions);

            var result = await _transactionService.GetDailyBalanceAsync(DateTime.Today);

            Assert.Equal(50, result);
        }
    }
}
