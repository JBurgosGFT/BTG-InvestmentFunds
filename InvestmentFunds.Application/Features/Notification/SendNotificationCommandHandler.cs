using InvestmentFunds.Application.Notifications;
using InvestmentFunds.Domain.Enums;
using MediatR;

namespace InvestmentFunds.Application.Features.Notification;

public class SendNotificationCommandHandler : IRequestHandler<SendNotificationCommand, Unit>
{
    private readonly INotificationStrategy _notificationStrategy;
    private readonly INotificationStrategy _email;
    private readonly INotificationStrategy _sms;
    public SendNotificationCommandHandler(INotificationStrategy notificationStrategy, INotificationStrategy email, INotificationStrategy sms)
    {
        _notificationStrategy = notificationStrategy;
        _email = email;
        _sms = sms;
    }

    public async Task<Unit> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
    {
        var subject = string.Empty;
        var body = string.Empty;
        switch (request.Type)
        {
            case TransactionType.Opening:
                subject = $"Suscripción a {request.FundName}";
                body = $"Se abrió su suscripción por ${request.Amount} al fondo {request.FundName}.";
                break;
            case TransactionType.Cancellation:
                subject = $"Cancelación de suscripción {request.FundName}";
                body = $"Se canceló su suscripción al fondo {request.FundName}. Se retornó ${request.Amount}.";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (request.NotificationPreference == NotificationPreference.SMS && !string.IsNullOrWhiteSpace(request.Phone))
            await _sms.SendAsync(request.Phone!, subject, body, cancellationToken);
        else if (!string.IsNullOrWhiteSpace(request.Email))
            await _email.SendAsync(request.Email!, subject, body, cancellationToken);
        await _notificationStrategy.SendAsync(request.Email!, subject, body, cancellationToken);
        return Unit.Value;
    }
}