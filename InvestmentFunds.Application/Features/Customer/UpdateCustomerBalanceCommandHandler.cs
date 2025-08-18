using InvestmentFunds.Application.Dtos;
using InvestmentFunds.Application.Interfaces;
using MediatR;

namespace InvestmentFunds.Application.Features.Customer;

public class UpdateCustomerBalanceCommandHandler : IRequestHandler<UpdateCustomerBalanceCommand, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;

    public UpdateCustomerBalanceCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerDto> Handle(UpdateCustomerBalanceCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetCustomerAsync(request.CustomerId, cancellationToken);
        if (customer == null)
            throw new KeyNotFoundException("Cliente no encontrado");

        customer.Balance = request.NewBalance;
        await _customerRepository.CreateOrUpdateCustomerAsync(customer, cancellationToken);

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
