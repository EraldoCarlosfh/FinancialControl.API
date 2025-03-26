using FinancialControl.Domain.Enums;

namespace FinancialControl.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public TransactionType Type { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }

        public void UpdateTransaction(TransactionType type, string description, decimal amount)
        {
            Type = type;
            Description = description;
            Amount = amount;
        }

        public void CreateTransaction(TransactionType type, string description, decimal amount)
        {
            Type = type;
            Description = description;
            Amount = amount;
        }
    }
}
