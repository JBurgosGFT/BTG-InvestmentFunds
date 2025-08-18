namespace InvestmentFunds.Application.Notifications;

public interface INotificationStrategy
{
    Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
}
