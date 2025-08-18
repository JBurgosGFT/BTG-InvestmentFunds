using InvestmentFunds.Domain.Entities;

public static class SubscriptionMapper
{
    public static SubscriptionDynamoModel ToDynamoModel(Subscription sub)
    {
        return new SubscriptionDynamoModel
        {
            SubscriptionId = sub.SubscriptionId,
            FundId = sub.FundId,
            SubscriptionDate = sub.SubscriptionDate,
            Amount = sub.Amount,
            IsActive = sub.IsActive,
        };
    }

    public static Subscription ToDomainEntity(SubscriptionDynamoModel model)
    {
        return new Subscription
        {
            SubscriptionId = model.SubscriptionId,
            FundId = model.FundId,
            SubscriptionDate = model.SubscriptionDate,
            Amount = model.Amount,
            IsActive = model.IsActive,
        };
    }
}