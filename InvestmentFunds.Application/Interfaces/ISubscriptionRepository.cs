using InvestmentFunds.Domain.Entities;

namespace InvestmentFunds.Application.Interfaces;

public interface ISubscriptionRepository
{
    Task CreateSubscriptionAsync(Subscription subscription, CancellationToken cancellationToken = default);
    Task<Subscription?> GetSubscriptionAsync(Guid subscriptionId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Subscription>> ListSubscriptionsByClientAsync(Guid clientId, CancellationToken cancellationToken = default);
    Task UpdateSubscriptionAsync(Subscription subscription, CancellationToken cancellationToken = default);
}
