using InvestmentFunds.Application.Features.Customer;
using InvestmentFunds.Application.Features.Fund;
using InvestmentFunds.Application.Features.Notification;
using InvestmentFunds.Application.Features.Transaction;
using InvestmentFunds.Application.Interfaces;
using InvestmentFunds.Domain.Entities;
using InvestmentFunds.Domain.Enums;
using MediatR;

namespace InvestmentFunds.Application.Features.Subscription;

public class SubscribeCommandHandler : IRequestHandler<SubscribeCommand, Guid>
{
    private readonly IMediator _mediator;
    private readonly ISubscriptionRepository _subscriptionRepository;

    public SubscribeCommandHandler(IMediator mediator, ISubscriptionRepository subscriptionRepository)
    {
        _mediator = mediator;
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<Guid> Handle(SubscribeCommand request, CancellationToken cancellationToken)
    {
        var customer = await _mediator.Send(new GetCustomerQuery { CustomerId = request.CustomerId }, cancellationToken);
        var fund = await _mediator.Send(new GetFundQuery { FundId = request.FundId }, cancellationToken);

        if (customer.Balance < fund.MinAmount)
            throw new InvalidOperationException($"No tiene saldo disponible para vincularse al fondo {fund.Name}");

        customer.Balance -= fund.MinAmount;

        var subscription = new Domain.Entities.Subscription
        {
            FundId = fund.FundId,
            Amount = fund.MinAmount,
            SubscriptionDate = DateTime.UtcNow,
            IsActive = true
        };

        await _subscriptionRepository.CreateSubscriptionAsync(subscription, cancellationToken);
        await _mediator.Send(new CreateTransactionCommand { CustomerId = customer.CustomerId, FundId = fund.FundId, Amount = fund.MinAmount, Type = TransactionType.Opening }, cancellationToken);
        await _mediator.Send(new UpdateCustomerBalanceCommand { CustomerId = customer.CustomerId, NewBalance = customer.Balance });
        await _mediator.Send(new SendNotificationCommand { FundName = fund.Name, Amount = fund.MinAmount.ToString(), NotificationPreference = customer.NotificationPreference, Email = customer.Email, Phone = customer.Phone, Type = TransactionType.Opening });

        return subscription.SubscriptionId;
    }
}
