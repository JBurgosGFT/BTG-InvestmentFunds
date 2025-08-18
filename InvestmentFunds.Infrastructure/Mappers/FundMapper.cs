using InvestmentFunds.Domain.Entities;

public static class FundMapper
{
    public static FundDynamoModel ToDynamoModel(Fund fund)
    {
        return new FundDynamoModel
        {
            FundId = fund.FundId,
            Name = fund.Name,
            Category = fund.Category,
            MinAmount = fund.MinAmount
        };
    }

    public static Fund ToDomainEntity(FundDynamoModel model)
    {
        return new Fund
        {
            FundId = model.FundId,
            Name = model.Name,
            Category = model.Category,
            MinAmount = model.MinAmount
        };
    }
}