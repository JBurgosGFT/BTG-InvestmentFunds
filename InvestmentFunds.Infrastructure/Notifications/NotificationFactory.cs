using InvestmentFunds.Application.Notifications;
using InvestmentFunds.Domain.Enums;

namespace InvestmentFunds.Infrastructure.Notifications;

public class NotificationFactory
{
     private readonly EmailNotificationStrategy _email;
    private readonly SMSNotificationStrategy _sms;
    public NotificationFactory(EmailNotificationStrategy email, SMSNotificationStrategy sms)
    {
        _email = email; _sms = sms;
    }

    public INotificationStrategy For(NotificationPreference pref)
        => pref == NotificationPreference.SMS ? (INotificationStrategy)_sms : _email;
}
