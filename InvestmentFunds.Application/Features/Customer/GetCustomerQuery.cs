using InvestmentFunds.Application.Dtos;
using MediatR;

namespace InvestmentFunds.Application.Features.Customer;

public class GetCustomerQuery : IRequest<CustomerDto>
{
    public Guid CustomerId { get; set; }
}
