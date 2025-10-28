using System;

namespace FinancialFlow.Domain.ValueObjects
{
    /// <summary>
    /// Value Object imutável que representa valores monetários.
    /// Record fornece igualdade por valor automaticamente.
    /// </summary>
    public sealed record Money
    {
        public decimal Amount { get; init; }
        public string Currency { get; init; }

        public Money(decimal amount, string currency)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be non-negative.");

            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Currency is required.", nameof(currency));

            Amount = amount;
            Currency = currency.Trim().ToUpperInvariant();
        }

        /// <summary>
        /// Cria uma instância de Money com valor zero.
        /// </summary>
        public static Money Zero(string currency) => new(0m, currency);

        /// <summary>
        /// Cria uma instância de Money em BRL.
        /// </summary>
        public static Money FromBRL(decimal amount) => new(amount, "BRL");

        /// <summary>
        /// Cria uma instância de Money em USD.
        /// </summary>
        public static Money FromUSD(decimal amount) => new(amount, "USD");

        /// <summary>
        /// Soma dois valores monetários (mesma moeda).
        /// </summary>
        public Money Add(Money other)
        {
            if (Currency != other.Currency)
                throw new InvalidOperationException("Cannot add money with different currencies.");

            return new Money(Amount + other.Amount, Currency);
        }

        /// <summary>
        /// Subtrai dois valores monetários (mesma moeda).
        /// </summary>
        public Money Subtract(Money other)
        {
            if (Currency != other.Currency)
                throw new InvalidOperationException("Cannot subtract money with different currencies.");

            return new Money(Amount - other.Amount, Currency);
        }

        public override string ToString() => $"{Amount:N2} {Currency}";
    }
}
