using InvestmentFunds.Domain.Enums;
using MediatR;

namespace InvestmentFunds.Application.Features.Transaction;

public class CreateTransactionCommand : IRequest<Guid>
{
    public Guid CustomerId { get; set; }
    public int FundId { get; set; }
    public int Amount { get; set; }
    public TransactionType Type { get; set; }
}
