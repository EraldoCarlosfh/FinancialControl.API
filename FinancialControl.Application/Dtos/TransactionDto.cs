using System.ComponentModel.DataAnnotations;
using FinancialControl.Application.Validations;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Enums;

namespace FinancialControl.Application.Dtos
{
    public class TransactionDto
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "O tipo da transação é obrigatório")]
        [EnumDataType(typeof(TransactionType), ErrorMessage = "Tipo de transação inválido")]
        public TransactionType Type { get; set; }

        [Required(ErrorMessage = "A data da transação é obrigatória")]
        [FutureDate(ErrorMessage = "Data da transação inválida")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "A descrição deve ter entre 3 e 100 caracteres")]
        public string Description { get; set; }

        [Required(ErrorMessage = "O valor é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero")]
        public decimal Amount { get; set; }

        // Métodos de conversão
        public Transaction ToEntity()
        {
            return new Transaction
            {
                Id = Id ?? Guid.NewGuid(),
                Type = Type,
                Date = Date,
                Description = Description,
                Amount = Amount
            };
        }

        public static TransactionDto FromEntity(Transaction entity)
        {
            if (entity == null) return null;

            return new TransactionDto
            {
                Id = entity.Id,
                Type = entity.Type,
                Date = entity.Date,
                Description = entity.Description,
                Amount = entity.Amount
            };
        }
    }
}