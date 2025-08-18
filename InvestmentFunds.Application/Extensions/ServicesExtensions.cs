using FluentValidation;
using InvestmentFunds.Application.Features.Customer;
using InvestmentFunds.Application.Features.Fund;
using InvestmentFunds.Application.Features.Notification;
using InvestmentFunds.Application.Features.Subscription;
using InvestmentFunds.Application.Features.Transaction;
using Microsoft.Extensions.DependencyInjection;

namespace InvestmentFunds.Application.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<SubscriptionCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<CustomerCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<CustomerQueryValidator>();
        services.AddValidatorsFromAssemblyContaining<FundQueryValidator>();
        services.AddValidatorsFromAssemblyContaining<TransactionCommandValidator>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<SubscribeCommand>());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<UpdateCustomerBalanceCommand>());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetCustomerQuery>());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetFundQuery>());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<SendNotificationCommand>());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<UnsubscribeCommand>());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateTransactionCommand>());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetAllTransactionsQuery>());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetTransactionsQuery>());

        return services;
    }
}
