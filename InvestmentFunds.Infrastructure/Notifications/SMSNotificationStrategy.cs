using InvestmentFunds.Application.Notifications;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

namespace InvestmentFunds.Infrastructure.Notifications;

public class SMSNotificationStrategy : INotificationStrategy
{
    private readonly IAmazonSimpleNotificationService _snsClient;

    public SMSNotificationStrategy(IAmazonSimpleNotificationService snsClient)
    {
        _snsClient = snsClient;
    }

    public async Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        var message = string.IsNullOrWhiteSpace(subject) ? body : $"{subject}: {body}";

        var request = new PublishRequest
        {
            Message = message,
            PhoneNumber = to
        };

        await _snsClient.PublishAsync(request, cancellationToken);
    }
}
