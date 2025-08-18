using Amazon.DynamoDBv2;
using Amazon.SecretsManager;
using Amazon.SimpleEmail;
using InvestmentFunds.Application.Interfaces;
using InvestmentFunds.Infrastructure.Notifications;
using InvestmentFunds.Infrastructure.Repositories;
using InvestmentFunds.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InvestmentFunds.Infrastructure.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // AWS SDK clients
        services.AddAWSService<IAmazonDynamoDB>();
        services.AddAWSService<IAmazonSecretsManager>();
        services.AddAWSService<IAmazonSimpleEmailService>();

        // Infrastructure services
        services.AddSingleton<DynamoDbService>();
        services.AddSingleton<AwsSecretManagerService>();

        // Repositories
        services.AddScoped<ICustomerRepository, DynamoDbCustomerRepository>();
        services.AddScoped<IFundRepository, DynamoDbFundRepository>();
        services.AddScoped<ISubscriptionRepository, DynamoDbSubscriptionRepository>();
        services.AddScoped<ITransactionRepository, DynamoDbTransactionRepository>();

        services.AddScoped<EmailNotificationStrategy>();
        services.AddScoped<SMSNotificationStrategy>();
        services.AddScoped<NotificationFactory>();

        return services;
    }
}
