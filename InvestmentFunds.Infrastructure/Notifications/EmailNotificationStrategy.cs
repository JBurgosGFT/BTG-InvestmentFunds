using InvestmentFunds.Application.Notifications;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace InvestmentFunds.Infrastructure.Notifications;

public class EmailNotificationStrategy : INotificationStrategy
{
    private readonly IAmazonSimpleEmailService _sesClient;
    private readonly string _fromAddress;

    public EmailNotificationStrategy(IAmazonSimpleEmailService sesClient, string fromAddress)
    {
        _sesClient = sesClient;
        _fromAddress = fromAddress;
    }

    public async Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        var sendRequest = new SendEmailRequest
        {
            Source = _fromAddress,
            Destination = new Destination
            {
                ToAddresses = new List<string> { to }
            },
            Message = new Message
            {
                Subject = new Content(subject),
                Body = new Body
                {
                    Html = new Content(body)
                }
            }
        };

        await _sesClient.SendEmailAsync(sendRequest, cancellationToken);
    }
}

