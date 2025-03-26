﻿namespace FinancialControl.Application.Configuration
{
    public class SmtpSettings
    {
        public string Server { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FromEmail { get; set; } = string.Empty;
        public string NotificationEmail { get; set; } = string.Empty;
        public bool EnableSsl { get; set; }
    }
}
