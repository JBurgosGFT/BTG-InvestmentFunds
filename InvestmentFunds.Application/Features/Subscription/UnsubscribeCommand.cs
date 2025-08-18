using MediatR;

namespace InvestmentFunds.Application.Features.Subscription;

public class UnsubscribeCommand : IRequest<Guid>
{
    public Guid SubscriptionId { get; set; }
    public Guid CustomerId { get; set; }
}
