using InvestmentFunds.Application.Notifications;

namespace InvestmentFunds.Infrastructure.Notifications;

public class SMSNotificationStrategy : INotificationStrategy
{
    public Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"SMS sent to: {to}");
        Console.WriteLine($"Subject: {subject}");
        Console.WriteLine($"Body: {body}");

        return Task.CompletedTask;
    }
}
