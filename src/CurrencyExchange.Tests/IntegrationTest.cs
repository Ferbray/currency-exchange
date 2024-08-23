using CurrencyExchange.Core.Entities;
using CurrencyExchange.Core.Interfaces;
using CurrencyExchange.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyExchange.Tests;

public class IntegrationTest
{
    private readonly IServiceScope _scope;
    private readonly WalletEntity _wallet = new()
    {
        Amount = 100000,
        Currency = Core.Constants.Currency.Rub
    };

    public IntegrationTest()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(_wallet);
        serviceCollection.AddSingleton<IExchangeService, ExchangeService>();
        serviceCollection.AddScoped<IWalletTransactionService, WalletTransactionService>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        _scope = serviceProvider.CreateScope();
    }

    [Fact]
    public void Replenishment_CorrectResult()
    {
        // Arrange
        var balanceWallet = GetService<WalletEntity>().Amount;
        var service = GetService<IWalletTransactionService>();
        var transaction = new TransactionEntity()
        {
            Amount = 100,
            Currency = Core.Constants.Currency.Eur
        };
        var rotate = GetService<IExchangeService>().GetRotate(GetService<WalletEntity>().Currency, transaction.Currency);

        // Act
        service.Replenishment(transaction);

        var actualAmount = GetService<WalletEntity>().Amount;
        var expectedAmount = balanceWallet + transaction.Amount * rotate;

        // Assert
        Assert.Equal(expectedAmount, actualAmount);
    }

    [Fact]
    public void CashOut_CorrectResult()
    {
        // Arrange
        var balanceWallet = GetService<WalletEntity>().Amount;
        var service = GetService<IWalletTransactionService>();
        var transaction = new TransactionEntity()
        {
            Amount = 100,
            Currency = Core.Constants.Currency.Eur
        };
        var rotate = GetService<IExchangeService>().GetRotate(GetService<WalletEntity>().Currency, transaction.Currency);

        // Act
        service.CashingOut(transaction);

        var actualAmount = GetService<WalletEntity>().Amount;
        var expectedAmount = balanceWallet - transaction.Amount * rotate;

        // Assert
        Assert.Equal(expectedAmount, actualAmount);
    }

    private T GetService<T>() where T : notnull => _scope.ServiceProvider.GetRequiredService<T>();
}