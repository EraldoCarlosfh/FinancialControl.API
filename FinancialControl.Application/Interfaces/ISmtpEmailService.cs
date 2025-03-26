namespace FinancialControl.Application.Interfaces
{
    public interface ISmtpEmailService
    {
        Task SendNegativeBalanceNotificationAsync(decimal currentBalance);
        Task SendEmailAsync(string to, string subject, string body);
    }
}
