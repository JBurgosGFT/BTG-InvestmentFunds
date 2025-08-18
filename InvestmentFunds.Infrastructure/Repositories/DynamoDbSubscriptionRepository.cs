using InvestmentFunds.Domain.Entities;
using InvestmentFunds.Application.Interfaces;
using Amazon.DynamoDBv2.DataModel;
using InvestmentFunds.Infrastructure.Services;
using Amazon.DynamoDBv2.DocumentModel;

namespace InvestmentFunds.Infrastructure.Repositories;

public class DynamoDbSubscriptionRepository : ISubscriptionRepository
{
    private readonly DynamoDBContext _context;
    public DynamoDbSubscriptionRepository(DynamoDbService dynamoDbService)
    {
        _context = dynamoDbService.Context;
    }

    public async Task CreateSubscriptionAsync(Subscription subscription, CancellationToken cancellationToken = default)
    {
        var dynamoModel = SubscriptionMapper.ToDynamoModel(subscription);
        await _context.SaveAsync(dynamoModel, cancellationToken);
    }

    public async Task<Subscription?> GetSubscriptionAsync(Guid subscriptionId, CancellationToken cancellationToken = default)
    {
        var dynamoModel = await _context.LoadAsync<SubscriptionDynamoModel>(subscriptionId, cancellationToken);
        return dynamoModel == null ? null : SubscriptionMapper.ToDomainEntity(dynamoModel);
    }

    public async Task<IEnumerable<Subscription>> ListSubscriptionsByClientAsync(Guid clientId, CancellationToken cancellationToken = default)
    {
        var conditions = new List<ScanCondition>
        {
            new ScanCondition("ClientId", ScanOperator.Equal, clientId)
        };
        return await _context.ScanAsync<Subscription>(conditions).GetRemainingAsync(cancellationToken);
    }

    public async Task UpdateSubscriptionAsync(Subscription subscription, CancellationToken cancellationToken = default)
    {
        var dynamoModel = SubscriptionMapper.ToDynamoModel(subscription);
        await _context.SaveAsync(dynamoModel, cancellationToken);
    }
}