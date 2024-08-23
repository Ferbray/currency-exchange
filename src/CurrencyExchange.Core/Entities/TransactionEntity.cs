namespace CurrencyExchange.Core.Entities;

public class TransactionEntity
{
    public decimal Amount { get; set; }
    public Constants.Currency Currency { get; set; }
}