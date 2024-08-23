using CurrencyExchange.Core.Entities;
using CurrencyExchange.Core.Interfaces;

namespace CurrencyExchange.Core.Services;

public class WalletTransactionService(WalletEntity wallet, IExchangeService exchangeService) : IWalletTransactionService
{
	public void CashingOut(TransactionEntity transaction)
	{
		transaction = exchangeService.Exchange(transaction, wallet.Currency);
		wallet.Amount -= transaction.Amount;
	}

	public void Replenishment(TransactionEntity transaction)
	{
		transaction = exchangeService.Exchange(transaction, wallet.Currency);
		wallet.Amount += transaction.Amount;
	}
}