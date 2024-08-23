using CurrencyExchange.Core.Entities;

namespace CurrencyExchange.Core.Interfaces;

public interface IExchangeService
{
    public TransactionEntity Exchange(TransactionEntity transaction, Constants.Currency currencyTo);
    public decimal GetRotate(Constants.Currency currencyFrom, Constants.Currency currencyTo);
}