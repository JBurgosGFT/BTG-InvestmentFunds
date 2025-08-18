using FluentValidation;
using InvestmentFunds.Application.Features.Customer;

public class CustomerQueryValidator : AbstractValidator<GetCustomerQuery>
{
    public CustomerQueryValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
    }
}