using Amazon.DynamoDBv2.DataModel;

[DynamoDBTable("Customers")]
public class CustomerDynamoModel
{
    [DynamoDBHashKey]
    public Guid CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public int Balance { get; set; }
    public int NotificationPreference { get; set; }

}