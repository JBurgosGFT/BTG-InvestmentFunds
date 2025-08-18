using InvestmentFunds.Domain.Entities;

public static class CustomerMapper
{
    public static CustomerDynamoModel ToDynamoModel(Customer customer)
    {
        return new CustomerDynamoModel
        {
            CustomerId = customer.CustomerId,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            Balance = customer.Balance,
            NotificationPreference = (int)customer.NotificationPreference
        };
    }

    public static Customer ToDomainEntity(CustomerDynamoModel model)
    {
        return new Customer
        {
            CustomerId = model.CustomerId,
            Name = model.Name,
            Email = model.Email,
            Phone = model.Phone,
            Balance = model.Balance,
            NotificationPreference = (InvestmentFunds.Domain.Enums.NotificationPreference)model.NotificationPreference
        };
    }
}