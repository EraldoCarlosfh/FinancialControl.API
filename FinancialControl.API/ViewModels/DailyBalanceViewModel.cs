using FinancialControl.Api.ViewModels;

namespace FinancialControl.API.ViewModels
{
    public class DailyBalanceViewModel : IViewModel
    {
        public string Date { get; set; } = string.Empty;
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal Balance { get; set; }
    }
}
