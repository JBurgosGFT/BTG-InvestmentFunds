using FluentValidation;
using InvestmentFunds.Application.Features.Customer;

public class CustomerCommandValidator : AbstractValidator<UpdateCustomerBalanceCommand>
{
    public CustomerCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.NewBalance).GreaterThan(0);
    }
}