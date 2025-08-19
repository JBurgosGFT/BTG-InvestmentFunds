using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.SecretsManager;
using Amazon.SimpleEmail;
using Amazon.SimpleNotificationService;
using InvestmentFunds.Application.Interfaces;
using InvestmentFunds.Infrastructure.Notifications;
using InvestmentFunds.Infrastructure.Repositories;
using InvestmentFunds.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InvestmentFunds.Infrastructure.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAWSService<IAmazonDynamoDB>();
        services.AddAWSService<IAmazonSecretsManager>();
        services.AddAWSService<IAmazonSimpleEmailService>();
        services.AddAWSService<IAmazonSimpleNotificationService>();

        services.AddSingleton<DynamoDbService>();
        services.AddSingleton<AwsSecretManagerService>();

        services.AddScoped<ICustomerRepository, DynamoDbCustomerRepository>();
        services.AddScoped<IFundRepository, DynamoDbFundRepository>();
        services.AddScoped<ISubscriptionRepository, DynamoDbSubscriptionRepository>();
        services.AddScoped<ITransactionRepository, DynamoDbTransactionRepository>();

        services.AddScoped<EmailNotificationStrategy>();
        services.AddScoped<SMSNotificationStrategy>();
        services.AddScoped<NotificationFactory>();

        var awsOptions = configuration.GetSection("AWS");

        var accessKey = awsOptions["AccessKey"];
        var secretKey = awsOptions["SecretKey"];
        var region = awsOptions["Region"];

        var awsCredentials = new BasicAWSCredentials(accessKey, secretKey);

        services.AddSingleton<IAmazonSimpleEmailService>(sp =>
            new AmazonSimpleEmailServiceClient(awsCredentials, Amazon.RegionEndpoint.GetBySystemName(region)));

        services.AddSingleton<IAmazonSimpleNotificationService>(sp =>
            new AmazonSimpleNotificationServiceClient(awsCredentials, Amazon.RegionEndpoint.GetBySystemName(region)));

        return services;
    }
}
