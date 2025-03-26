using FinancialControl.Application.Dtos;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace FinancialControl.Tests.Dtos
{
    public class TransactionDtoTests
    {
        [Fact]
        public void ToEntity_ShouldConvertCorrectly()
        {
            var transactionDto = new TransactionDto
            {
                Type = TransactionType.Receita,
                Date = DateTime.Now,
                Description = "Salário",
                Amount = 1000,
            };

            var entity = transactionDto.ToEntity();
  
            Assert.Equal(transactionDto.Type, entity.Type);
            Assert.Equal(transactionDto.Date, entity.Date);
            Assert.Equal(transactionDto.Description, entity.Description);
            Assert.Equal(transactionDto.Amount, entity.Amount);
            Assert.NotEqual(Guid.Empty, entity.Id);
        }

        [Fact]
        public void FromEntity_ShouldConvertCorrectly()
        {
            var entity = new Transaction
            {
                Id = Guid.NewGuid(),
                Type = TransactionType.Despesa,
                Date = DateTime.Now.AddDays(-1),
                Description = "Mercado",
                Amount = 150,
            };

            var transactionDto = TransactionDto.FromEntity(entity);

            Assert.Equal(entity.Id, transactionDto.Id);
            Assert.Equal(entity.Type, transactionDto.Type);
            Assert.Equal(entity.Date, transactionDto.Date);
            Assert.Equal(entity.Description, transactionDto.Description);
            Assert.Equal(entity.Amount, transactionDto.Amount);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("AB", false)]
        [InlineData("Descrição válida", true)]
        public void DescriptionValidation(string description, bool isValid)
        {
            var transactionDto = new TransactionDto
            {
                Type = TransactionType.Receita,
                Date = DateTime.Now,
                Description = description,
                Amount = 100
            };

            var context = new ValidationContext(transactionDto);
            var results = new System.Collections.Generic.List<ValidationResult>();

            var success = Validator.TryValidateObject(transactionDto, context, results, true);

            Assert.Equal(isValid, success);
            if (!isValid)
            {
                Assert.Contains(results, r => r.MemberNames.Contains("Description"));
            }
        }

        [Fact]
        public void FutureDateValidation_ShouldFailForDistantFuture()
        {
            var transactionDto = new TransactionDto
            {
                Type = TransactionType.Receita,
                Date = DateTime.Now.AddDays(2),
                Description = "Teste",
                Amount = 100
            };

            var context = new ValidationContext(transactionDto);
            var results = new List<ValidationResult>();
     
            var success = Validator.TryValidateObject(transactionDto, context, results, true);
         
            Assert.False(success);
            Assert.Contains(results, r => r.ErrorMessage.Contains("futuro distante"));
        }
    }
}