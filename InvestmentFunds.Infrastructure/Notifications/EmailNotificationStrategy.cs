using InvestmentFunds.Application.Notifications;

namespace InvestmentFunds.Infrastructure.Notifications;

public class EmailNotificationStrategy : INotificationStrategy
{
    public Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Email sent to: {to}");
        Console.WriteLine($"Subject: {subject}");
        Console.WriteLine($"Body: {body}");
        
        return Task.CompletedTask;
    }
}

