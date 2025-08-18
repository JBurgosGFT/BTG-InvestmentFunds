using InvestmentFunds.Application.Dtos;
using InvestmentFunds.Domain.Enums;
using MediatR;

namespace InvestmentFunds.Application.Features.Transaction;

public class GetAllTransactionsQuery : IRequest<List<TransactionDto>>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TransactionType Type { get; set; }
}
