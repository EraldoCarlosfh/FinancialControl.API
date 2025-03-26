using FinancialControl.Application.Configuration;
using FinancialControl.Application.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace FinancialControl.Tests.Services
{
    public class SmtpEmailServiceTests
    {
        private readonly Mock<ILogger<SmtpEmailService>> _loggerMock;
        private readonly SmtpSettings _smtpSettings;
        private readonly SmtpEmailService _emailService;

        public SmtpEmailServiceTests()
        {
            _loggerMock = new Mock<ILogger<SmtpEmailService>>();

            _smtpSettings = new SmtpSettings
            {
                Server = "smtp.example.com",
                Port = 587,
                Username = "seu-email@example.com",
                Password = "sua-senha",
                FromEmail = "noreply@financialcontrol.com",
                EnableSsl = true,
                NotificationEmail = "usuario@example.com",
            };

            var optionsMock = new Mock<IOptions<SmtpSettings>>();
            optionsMock.Setup(o => o.Value).Returns(_smtpSettings);

            _emailService = new SmtpEmailService(optionsMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenSmtpSettingsIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new SmtpEmailService(null, _loggerMock.Object));

            Assert.Equal("smtpSettings", exception.ParamName);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenLoggerIsNull()
        {
            var optionsMock = new Mock<IOptions<SmtpSettings>>();
            optionsMock.Setup(o => o.Value).Returns(_smtpSettings);

            var exception = Assert.Throws<ArgumentNullException>(() =>
                new SmtpEmailService(optionsMock.Object, null));

            Assert.Equal("logger", exception.ParamName);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenSmtpSettingsInvalid()
        {
            var invalidSmtpSettings = new SmtpSettings
            {
                Server = "",
                Username = "",
                FromEmail = ""
            };

            var optionsMock = new Mock<IOptions<SmtpSettings>>();
            optionsMock.Setup(o => o.Value).Returns(invalidSmtpSettings);

            var exception = Assert.Throws<ArgumentException>(() =>
                new SmtpEmailService(optionsMock.Object, _loggerMock.Object));

            Assert.Contains("Servidor SMTP não configurado", exception.Message);
        }

        [Fact]
        public async Task SendNegativeBalanceNotificationAsync_ShouldThrowException_WhenNotificationEmailIsNotConfigured()
        {
            var invalidSmtpSettings = new SmtpSettings
            {
                Server =  "smtp.example.com",
                Port =  587,
                Username =  "seu-email@example.com",
                Password =  "sua-senha",
                FromEmail =  "noreply@financialcontrol.com",
                EnableSsl =  true,
                NotificationEmail = null,
            };

            var optionsMock = new Mock<IOptions<SmtpSettings>>();
            optionsMock.Setup(o => o.Value).Returns(invalidSmtpSettings);

            var emailService = new SmtpEmailService(optionsMock.Object, _loggerMock.Object);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                emailService.SendNegativeBalanceNotificationAsync(-100));

            Assert.Equal("Email de notificação não configurado", exception.Message);
        }

        [Fact]
        public async Task SendNegativeBalanceNotificationAsync_ShouldLogMessage_WhenEmailIsSent()
        {
            await _emailService.SendNegativeBalanceNotificationAsync(-150);

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Notificação de saldo negativo enviada para")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task SendEmailAsync_ShouldThrowException_WhenToIsEmpty()
        {
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _emailService.SendEmailAsync("", "Assunto", "Corpo"));

            Assert.Equal("Destinatário não pode ser vazio", exception.Message);
        }

        [Fact]
        public async Task SendEmailAsync_ShouldThrowException_WhenSubjectIsEmpty()
        {
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _emailService.SendEmailAsync("test@test.com", "", "Corpo"));

            Assert.Equal("Assunto não pode ser vazio", exception.Message);
        }

        [Fact]
        public async Task SendEmailAsync_ShouldThrowException_WhenBodyIsEmpty()
        {
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _emailService.SendEmailAsync("test@test.com", "Assunto", ""));

            Assert.Equal("Corpo do email não pode ser vazio", exception.Message);
        }

        [Fact]
        public void CreateMailMessage_ShouldReturnValidMailMessage()
        {
            var message = _emailService.CreateMailMessage("recipient@test.com", "Test Subject", "Test Body");

            Assert.NotNull(message);
            Assert.Equal("recipient@test.com", message.To[0].Address);
            Assert.Equal("Test Subject", message.Subject);
            Assert.Equal("Test Body", message.Body);
            Assert.Equal(_smtpSettings.FromEmail, message.From.Address);
        }
    }
}
