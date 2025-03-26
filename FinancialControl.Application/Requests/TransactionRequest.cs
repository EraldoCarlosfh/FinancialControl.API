using FinancialControl.Domain.Enums;

namespace FinancialControl.Application.Requests
{
    public class TransactionRequest
    {
        public Guid Id { get; set; }       
        public TransactionType Type { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }

    }
}