using FinancialControl.Infra.Interfaces;
using FinancialControl.Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialControl.Infra.Module
{
    public static class RepositoriesModule
    {
        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<ITransactionRepository, TransactionRepository>();
        }
    }
}
