using FinancialControl.Api.ViewModels;
using FinancialControl.Domain.Enums;

namespace FinancialControl.API.ViewModels
{
    public class TransactionViewModel : IViewModel
    {
        public string Id { get; set; }
        public TransactionType Type { get; set; }
        public string Date { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
