using InvestmentFunds.Application.Dtos;
using MediatR;

namespace InvestmentFunds.Application.Features.Fund;

public class GetFundQuery : IRequest<FundDto>
{
    public int FundId { get; set; }
}

