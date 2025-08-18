using InvestmentFunds.Application.Dtos;
using MediatR;

namespace InvestmentFunds.Application.Features.Customer;

public class UpdateCustomerBalanceCommand : IRequest<CustomerDto>
{
    public Guid CustomerId { get; set; }
    public int NewBalance { get; set; }
}

