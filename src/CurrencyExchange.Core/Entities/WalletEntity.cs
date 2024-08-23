namespace CurrencyExchange.Core.Entities;

public class WalletEntity
{
    public decimal Amount { get; set; }
    public Constants.Currency Currency { get; set; }
}