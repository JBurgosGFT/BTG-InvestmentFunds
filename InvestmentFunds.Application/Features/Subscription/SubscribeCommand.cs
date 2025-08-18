using MediatR;

namespace InvestmentFunds.Application.Features.Subscription;

public class SubscribeCommand : IRequest<Guid>
{
    public int FundId { get; set; }
    public Guid CustomerId { get; set; }
}
