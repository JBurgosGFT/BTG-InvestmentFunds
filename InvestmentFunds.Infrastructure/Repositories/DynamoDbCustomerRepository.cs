using InvestmentFunds.Domain.Entities;
using InvestmentFunds.Application.Interfaces;
using Amazon.DynamoDBv2.DataModel;
using InvestmentFunds.Infrastructure.Services;


namespace InvestmentFunds.Infrastructure.Repositories;

public class DynamoDbCustomerRepository : ICustomerRepository
{
    private readonly DynamoDBContext _context;
    public DynamoDbCustomerRepository(DynamoDbService dynamoDbService)
    {
        _context = dynamoDbService.Context;
    }

    public async Task CreateOrUpdateCustomerAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        var dynamoModel = CustomerMapper.ToDynamoModel(customer);
        await _context.SaveAsync(dynamoModel, cancellationToken);
    }

    public async Task<Customer?> GetCustomerAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var dynamoModel = await _context.LoadAsync<CustomerDynamoModel>(customerId, cancellationToken);
        return dynamoModel == null ? null : CustomerMapper.ToDomainEntity(dynamoModel);
    }
}