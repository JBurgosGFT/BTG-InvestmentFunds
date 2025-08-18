using FluentValidation;
using InvestmentFunds.Application.Features.Transaction;

public class TransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public TransactionCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.FundId).GreaterThan(0);
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Type).NotEmpty();
    }
}