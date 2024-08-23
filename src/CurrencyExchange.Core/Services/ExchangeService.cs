using CurrencyExchange.Core.Entities;
using CurrencyExchange.Core.Interfaces;

namespace CurrencyExchange.Core.Services;

public class ExchangeService : IExchangeService
{
    private readonly Dictionary<Constants.Currency, List<Node>> CurrencyNodes = new()
    {
        { Constants.Currency.Rub, new List<Node>() { new() { Currency = Constants.CrossCurrency, Rotate = 0.3M } } },
        { Constants.Currency.Eur, new List<Node>() { new() { Currency = Constants.CrossCurrency, Rotate = 1.1M } } },
        { Constants.Currency.Usd, new List<Node>() { new() { Currency = Constants.CrossCurrency, Rotate = 0 } } },
    };

    public TransactionEntity Exchange(TransactionEntity transaction, Constants.Currency currency)
    {
        if (transaction.Currency == currency)
        {
            return transaction;
        }

        var nodeActual = GetNode(transaction.Currency, currency);

        return new TransactionEntity()
        {
            Currency = currency,
            Amount = transaction.Amount * nodeActual.Rotate
        };
    }

    public decimal GetRotate(Constants.Currency currencyFrom, Constants.Currency currencyTo) =>
        GetNode(currencyFrom, currencyTo).Rotate;

    private Node GetNode(Constants.Currency currencyFrom, Constants.Currency currencyTo)
    {
        var nodesActual = CurrencyNodes.GetValueOrDefault(currencyFrom)!;
        var nodeActual = nodesActual.FirstOrDefault(x => x.Currency == currencyTo) ??
            CreateNode(nodesActual, currencyTo);

        return nodeActual;
    }

    private Node CreateNode(List<Node> nodesActual, Constants.Currency currencyTo)
    {
        var nodesExpected = CurrencyNodes.GetValueOrDefault(currencyTo)!;

        var crossActual = nodesActual.FirstOrDefault(x => x.Currency == Constants.CrossCurrency)!;
        var crossExpected = nodesExpected.FirstOrDefault(x => x.Currency == Constants.CrossCurrency)!;

        var rotate = Math.Abs(crossActual.Rotate - crossExpected.Rotate);

        var node = new Node()
        {
            Currency = currencyTo,
            Rotate = rotate
        };

        nodesActual.Add(node);

        return node;
    }

    class Node
    {
        public Constants.Currency Currency { get; set; }
        public decimal Rotate { get; set; }
    }
}