using System;
using FinancialFlow.Domain.Enums;
using FinancialFlow.Domain.ValueObjects;

namespace FinancialFlow.Domain.Entities
{
    /// <summary>
    /// Aggregate Root que representa uma transação financeira (receita ou despesa).
    /// </summary>
    public class FinancialTransaction : Entity
    {
        public Guid UserId { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public DateTimeOffset TransactionDate { get; private set; }
        public TransactionType Type { get; private set; }
        public Money Value { get; private set; } = null!;

        // Construtor protegido para EF Core e herança
        protected FinancialTransaction() { }

        private FinancialTransaction(
            Guid userId,
            Money value,
            TransactionType type,
            string description,
            DateTimeOffset transactionDate)
            : base()
        {
            UserId = userId;
            Value = value ?? throw new ArgumentNullException(nameof(value));
            Type = type;
            Description = description ?? string.Empty;
            TransactionDate = transactionDate;
        }

        /// <summary>
        /// Método factory para criar uma nova transação financeira com validações.
        /// </summary>
        public static FinancialTransaction Create(
            Guid userId,
            Money value,
            TransactionType type,
            string description,
            DateTimeOffset? transactionDate = null)
        {
            // Validações básicas
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId is required.", nameof(userId));

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (value.Amount < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Transaction value must be non-negative.");

            var txDate = transactionDate ?? DateTimeOffset.UtcNow;
            var transaction = new FinancialTransaction(userId, value, type, description, txDate);

            // Adicionar Domain Event se necessário
            // transaction.AddDomainEvent(new TransactionCreatedEvent(transaction));

            return transaction;
        }

        /// <summary>
        /// Atualiza a descrição da transação.
        /// </summary>
        public void UpdateDescription(string description)
        {
            Description = description ?? string.Empty;
        }

        /// <summary>
        /// Atualiza o valor da transação.
        /// </summary>
        public void UpdateValue(Money value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            Value = value;
        }

        /// <summary>
        /// Atualiza a data da transação.
        /// </summary>
        public void UpdateTransactionDate(DateTimeOffset transactionDate)
        {
            TransactionDate = transactionDate;
        }
    }
}
