using InvestmentFunds.Domain.Enums;
using MediatR;

namespace InvestmentFunds.Application.Features.Notification;

public class SendNotificationCommand : IRequest<Unit>
{
    public string FundName { get; set; } = string.Empty;
    public string Amount { get; set; } = string.Empty;
    public NotificationPreference NotificationPreference { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public TransactionType Type { get; set; }
}
