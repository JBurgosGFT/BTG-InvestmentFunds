using Amazon.DynamoDBv2.DataModel;

[DynamoDBTable("Subscriptions")]
public class SubscriptionDynamoModel
{
    [DynamoDBHashKey]
    public Guid SubscriptionId { get; set; }
    public int FundId { get; set; }
    public DateTime SubscriptionDate { get; set; }
    public int Amount { get; set; }
    public bool IsActive { get; set; }

    [DynamoDBGlobalSecondaryIndexHashKey("CustomerId-index")]
    public Guid CustomerId { get; set; }

    [DynamoDBGlobalSecondaryIndexHashKey("FundId-index")]
    public int FundIdGSI => FundId;
}