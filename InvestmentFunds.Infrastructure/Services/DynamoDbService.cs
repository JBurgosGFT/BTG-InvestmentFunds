using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Microsoft.Extensions.Configuration;

namespace InvestmentFunds.Infrastructure.Services;

public class DynamoDbService
{
    public DynamoDBContext Context { get; }
    public DynamoDbService(IConfiguration config)
    {
        var endpoint = config["AWS:DynamoDbEndpoint"];
        var region = config["AWS:Region"];
        var clientConfig = new AmazonDynamoDBConfig { ServiceURL = endpoint, RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(region) };
        var client = new AmazonDynamoDBClient(clientConfig);
        
        var contextBuilder = new DynamoDBContextBuilder();
        Context = contextBuilder
            .WithDynamoDBClient(() =>client)
            .Build();
    }
}
