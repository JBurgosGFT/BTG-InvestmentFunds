using InvestmentFunds.Application.Features.Customer;
using InvestmentFunds.Application.Features.Fund;
using InvestmentFunds.Application.Features.Notification;
using InvestmentFunds.Application.Features.Transaction;
using InvestmentFunds.Application.Interfaces;
using InvestmentFunds.Domain.Enums;
using MediatR;

namespace InvestmentFunds.Application.Features.Subscription;

public class UnsubscribeCommandHandler : IRequestHandler<UnsubscribeCommand, Guid>
{
    private readonly IMediator _mediator;
    private readonly ISubscriptionRepository _subscriptionRepository;

    public UnsubscribeCommandHandler(IMediator mediator, ISubscriptionRepository subscriptionRepository)
    {
        _mediator = mediator;
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<Guid> Handle(UnsubscribeCommand request, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionRepository.GetSubscriptionAsync(request.SubscriptionId, cancellationToken)
                  ?? throw new KeyNotFoundException("Suscripción no encontrada");

        if (!subscription.IsActive)
            return subscription.SubscriptionId;

        if (subscription.FundId == 0) throw new InvalidOperationException("Fondo invalido");

        var customer = await _mediator.Send(new GetCustomerQuery { CustomerId = request.CustomerId }, cancellationToken)
                      ?? throw new KeyNotFoundException("Cliente no encontrado");

        var fund = await _mediator.Send(new GetFundQuery { FundId = subscription.FundId }, cancellationToken)
                   ?? throw new KeyNotFoundException("Fondo no encontrado");

        subscription.Cancel();
        customer.Balance += subscription.Amount;

        await _subscriptionRepository.UpdateSubscriptionAsync(subscription, cancellationToken);
        await _mediator.Send(new CreateTransactionCommand { CustomerId = customer.CustomerId, FundId = fund.FundId, Amount = subscription.Amount, Type = TransactionType.Cancellation }, cancellationToken);
        await _mediator.Send(new UpdateCustomerBalanceCommand { CustomerId = customer.CustomerId, NewBalance = customer.Balance });
        await _mediator.Send(new SendNotificationCommand { FundName = fund.Name, Amount = fund.MinAmount.ToString(), NotificationPreference = customer.NotificationPreference, Email = customer.Email, Phone = customer.Phone, Type = TransactionType.Cancellation });

        return subscription.SubscriptionId;
    }
}
