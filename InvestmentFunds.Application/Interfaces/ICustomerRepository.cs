using InvestmentFunds.Domain.Entities;

namespace InvestmentFunds.Application.Interfaces;

public interface ICustomerRepository
{
    Task<Customer?> GetCustomerAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task CreateOrUpdateCustomerAsync(Customer customer, CancellationToken cancellationToken = default);
}
