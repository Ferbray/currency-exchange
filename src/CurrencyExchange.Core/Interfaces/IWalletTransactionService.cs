using CurrencyExchange.Core.Entities;

namespace CurrencyExchange.Core.Interfaces;

public interface IWalletTransactionService
{
    public void Replenishment(TransactionEntity transaction);
    public void CashingOut(TransactionEntity transaction);
}