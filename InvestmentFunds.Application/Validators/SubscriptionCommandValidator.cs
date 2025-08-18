using FluentValidation;
using InvestmentFunds.Application.Features.Subscription;

public class SubscriptionCommandValidator : AbstractValidator<SubscribeCommand>
{
    public SubscriptionCommandValidator()
    {
        RuleFor(x => x.FundId).GreaterThan(0);
        RuleFor(x => x.CustomerId).NotEmpty();
    }
}