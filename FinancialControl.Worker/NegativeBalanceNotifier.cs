using FinancialControl.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FinancialControl.Worker
{
    public class NegativeBalanceNotifier : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<NegativeBalanceNotifier> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(24);

        public NegativeBalanceNotifier(IServiceScopeFactory scopeFactory, ILogger<NegativeBalanceNotifier> logger)
        {
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var transactionService = scope.ServiceProvider.GetRequiredService<ITransactionService>();
                    var emailService = scope.ServiceProvider.GetRequiredService<ISmtpEmailService>();

                    var negativeDays = await transactionService.GetNegativeBalanceDaysAsync();

                    var todayNegativeDay = negativeDays.FirstOrDefault(d => d.Date.Date == DateTime.Today);

                    if (todayNegativeDay != null)
                    {
                        await emailService.SendNegativeBalanceNotificationAsync(todayNegativeDay.TotalExpenses);
                        _logger.LogInformation($"Negative balance notification sent. Amount: {todayNegativeDay.TotalExpenses}");
                    }                   
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }
        }
    }

}
