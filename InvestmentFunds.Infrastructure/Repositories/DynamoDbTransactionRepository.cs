using InvestmentFunds.Domain.Entities;
using InvestmentFunds.Application.Interfaces;
using Amazon.DynamoDBv2.DataModel;
using InvestmentFunds.Infrastructure.Services;
using InvestmentFunds.Domain.Enums;
using Amazon.DynamoDBv2.DocumentModel;

namespace InvestmentFunds.Infrastructure.Repositories;

public class DynamoDbTransactionRepository : ITransactionRepository
{
    private readonly DynamoDBContext _context;
    public DynamoDbTransactionRepository(DynamoDbService dynamoDbService)
    {
        _context = dynamoDbService.Context;
    }

    public async Task<Guid> CreateTransactionAsync(Transaction transaction, CancellationToken cancellationToken = default)
    {
        var dynamoModel = TransactionMapper.ToDynamoModel(transaction);
        await _context.SaveAsync(dynamoModel, cancellationToken);
        return dynamoModel.TransactionId;
    }

    public async Task<IEnumerable<Transaction>> ListAllTransactionsAsync(DateTime startDate, DateTime endDate, TransactionType type, CancellationToken cancellationToken = default)
    {
        var conditions = new List<ScanCondition>
        {
            new ScanCondition("TransactionDate", ScanOperator.Between, startDate, endDate),
            new ScanCondition("TransactionType", ScanOperator.Equal, type)
        };

        var search = _context.ScanAsync<TransactionDynamoModel>(conditions);
        var results = await search.GetRemainingAsync(cancellationToken);
        return results.Select(TransactionMapper.ToDomainEntity);
    }

    public async Task<IEnumerable<Transaction>> ListTransactionsByClientAsync(Guid clientId, TransactionType type, CancellationToken cancellationToken = default)
    {
        var conditions = new List<ScanCondition>
        {
            new ScanCondition("ClientId", ScanOperator.Equal, clientId),
            new ScanCondition("TransactionType", ScanOperator.Equal, type)
        };

        var search = _context.ScanAsync<TransactionDynamoModel>(conditions);
        var results = await search.GetRemainingAsync(cancellationToken);
        return results.Select(TransactionMapper.ToDomainEntity);
    }
}