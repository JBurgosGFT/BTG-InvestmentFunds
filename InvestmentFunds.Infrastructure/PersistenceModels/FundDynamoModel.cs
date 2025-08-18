using Amazon.DynamoDBv2.DataModel;

[DynamoDBTable("Funds")]
public class FundDynamoModel
{
    [DynamoDBHashKey]
    public int FundId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int MinAmount { get; set; }
}