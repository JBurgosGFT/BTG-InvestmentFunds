using Amazon.DynamoDBv2.DataModel;
using InvestmentFunds.Domain.Enums;

[DynamoDBTable("Transactions")]
public class TransactionDynamoModel
{
    [DynamoDBHashKey]
    public Guid TransactionId { get; set; }

    [DynamoDBGlobalSecondaryIndexHashKey("CustomerId-index")]
    public Guid CustomerId { get; set; } // GSI para consultar transacciones por cliente

    [DynamoDBGlobalSecondaryIndexHashKey("FundId-index")]
    public int FundId { get; set; } // GSI para consultar transacciones por fondo

    public DateTime TransactionDate { get; set; }
    public int Amount { get; set; }
    public TransactionType Type { get; set; }

}