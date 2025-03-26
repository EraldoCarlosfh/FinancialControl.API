using FinancialControl.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using FinancialControl.Application.Configuration;

namespace FinancialControl.Application.Services
{
    public class SmtpEmailService : ISmtpEmailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly ILogger<SmtpEmailService> _logger;

        public SmtpEmailService(IOptions<SmtpSettings> smtpSettings, ILogger<SmtpEmailService> logger)
        {
            _smtpSettings = smtpSettings?.Value ?? throw new ArgumentNullException(nameof(smtpSettings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            ValidateSmtpSettings();
        }

        public async Task SendNegativeBalanceNotificationAsync(decimal currentBalance)
        {
            if (string.IsNullOrWhiteSpace(_smtpSettings.NotificationEmail))
                throw new InvalidOperationException("Email de notificação não configurado");

            try
            {
                var formattedBalance = currentBalance.ToString("C");
                var subject = $"[Alerta Financeiro] Saldo Negativo: {formattedBalance}";

                var body = $@"<h1>Alerta de Saldo Negativo</h1>
                            <p>Seu controle financeiro está com saldo negativo.</p>
                            <p><strong>Saldo atual:</strong> {formattedBalance}</p>
                            <p>Por favor, revise suas transações recentes e considere ajustar seus gastos.</p>
                            <p>Atenciosamente,<br>Equipe de Controle Financeiro</p>";

                Console.WriteLine(body);

                //Envia email de notificação
                //await SendEmailAsync(
                //    _smtpSettings.NotificationEmail,
                //    subject,
                //    body);

                _logger.LogInformation("Notificação de saldo negativo enviada para {Email}",
                    _smtpSettings.NotificationEmail);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao enviar notificação de saldo negativo");
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(to))
                throw new ArgumentException("Destinatário não pode ser vazio");

            if (string.IsNullOrWhiteSpace(subject))
                throw new ArgumentException("Assunto não pode ser vazio");

            if (string.IsNullOrWhiteSpace(body))
                throw new ArgumentException("Corpo do email não pode ser vazio");

            try
            {
                using (var client = CreateSmtpClient())
                using (var message = CreateMailMessage(to, subject, body))
                {
                    await client.SendMailAsync(message);
                    _logger.LogDebug("Email enviado para {To} com assunto '{Subject}'", to, subject);
                }
            }
            catch (SmtpException smtpEx)
            {
                _logger.LogError(smtpEx, "Erro SMTP ao enviar email para {To}", to);
                throw new EmailException("Falha ao enviar email via SMTP", smtpEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao enviar email para {To}", to);
                throw new EmailException("Falha ao enviar email", ex);
            }
        }

        private SmtpClient CreateSmtpClient()
        {
            return new SmtpClient(_smtpSettings.Server, _smtpSettings.Port)
            {
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                EnableSsl = _smtpSettings.EnableSsl,
                Timeout = 10000
            };
        }

        public MailMessage CreateMailMessage(string to, string subject, string body)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_smtpSettings.FromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = body.Contains("<") && body.Contains(">"),
                Priority = MailPriority.Normal
            };

            message.To.Add(to);

            return message;
        }

        private void ValidateSmtpSettings()
        {
            if (string.IsNullOrWhiteSpace(_smtpSettings.Server))
                throw new ArgumentException("Servidor SMTP não configurado");

            if (string.IsNullOrWhiteSpace(_smtpSettings.Username))
                throw new ArgumentException("Usuário SMTP não configurado");

            if (string.IsNullOrWhiteSpace(_smtpSettings.FromEmail))
                throw new ArgumentException("Email remetente não configurado");
        }
    }

    public class EmailException : Exception
    {
        public EmailException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}