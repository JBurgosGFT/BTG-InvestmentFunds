using FluentValidation;
using InvestmentFunds.Application.Features.Fund;

public class FundQueryValidator : AbstractValidator<GetFundQuery>
{
    public FundQueryValidator()
    {
        RuleFor(x => x.FundId).NotEmpty();
        RuleFor(x => x.FundId).GreaterThan(0);
    }
}