using InvestmentFunds.Application.Dtos;
using InvestmentFunds.Domain.Enums;
using MediatR;

namespace InvestmentFunds.Application.Features.Transaction;

public class GetTransactionsQuery : IRequest<List<TransactionDto>>
{
    public Guid CustomerId { get; set; }
    public TransactionType Type { get; set; }
}
