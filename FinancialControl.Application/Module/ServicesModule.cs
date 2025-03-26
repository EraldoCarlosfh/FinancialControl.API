using FinancialControl.Application.Interfaces;
using FinancialControl.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialControl.Application.Module
{
    public static class ServicesModule
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ISmtpEmailService, SmtpEmailService>();
        }
    }
}
