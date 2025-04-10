﻿using Microsoft.EntityFrameworkCore;

namespace FinancialControl.API.Api.Notifications
{
    [Keyless]
    public sealed class Notification
    {
        public Notification(string property, string message)
        {
            Property = property;
            Message = message;
        }

        public string Property { get; private set; }
        public string Message { get; private set; }
    }
}
