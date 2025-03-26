namespace FinancialControl.Domain.Dtos
{
    public class DailyBalanceResultDto
    {
        public DateTime Date { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal Balance { get; set; }
    }
}
