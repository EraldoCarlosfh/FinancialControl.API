using FinancialControl.Domain.Enums;

namespace FinancialControl.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public TransactionType Type { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }

        public void UpdateTransaction(TransactionType type, string description, decimal amount)
        {
            Type = type;
            Description = description;
            Amount = amount;
        }
    }
}
