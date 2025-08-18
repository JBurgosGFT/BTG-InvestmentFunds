using InvestmentFunds.Application.Dtos;
using InvestmentFunds.Application.Interfaces;
using MediatR;

namespace InvestmentFunds.Application.Features.Customer;

public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerDto> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetCustomerAsync(request.CustomerId, cancellationToken);
        if (customer == null)
            throw new Exception($"Cliente no existe.");
        
        var customerDto = new CustomerDto
        {
            CustomerId = customer.CustomerId,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            Balance = customer.Balance,
            NotificationPreference = customer.NotificationPreference
        };

        return customerDto;
    }
}
