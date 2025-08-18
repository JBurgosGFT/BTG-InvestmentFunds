using InvestmentFunds.Domain.Entities;
using InvestmentFunds.Application.Interfaces;
using Amazon.DynamoDBv2.DataModel;
using InvestmentFunds.Infrastructure.Services;

namespace InvestmentFunds.Infrastructure.Repositories;

public class DynamoDbFundRepository : IFundRepository
{
    private readonly DynamoDBContext _context;
    public DynamoDbFundRepository(DynamoDbService dynamoDbService)
    {
        _context = dynamoDbService.Context;
    }

    public async Task<Fund?> GetFundAsync(int fundId, CancellationToken cancellationToken = default)
    {
        var dynamoModel = await _context.LoadAsync<FundDynamoModel>(fundId, cancellationToken);
        return dynamoModel == null ? null : FundMapper.ToDomainEntity(dynamoModel);
    }

    public async Task<IEnumerable<Fund>> ListFundsAsync(CancellationToken cancellationToken = default)
    { 
        var conditions = new List<ScanCondition>();
        var dynamoModels = await _context.ScanAsync<FundDynamoModel>(conditions).GetRemainingAsync(cancellationToken);
        return dynamoModels.Select(FundMapper.ToDomainEntity);
    }
}