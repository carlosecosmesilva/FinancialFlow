using System;
using FinancialFlow.Domain.Enums;
using FinancialFlow.Domain.Events;
using FinancialFlow.Domain.Exceptions;
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
        public string? Category { get; private set; }
        public string? Notes { get; private set; }

        // Construtor protegido para EF Core
        protected FinancialTransaction(Guid userId)
        {
            Description = string.Empty;
            Value = null!;
            UserId = userId;
        }

        private FinancialTransaction(
            Guid userId,
            Money value,
            TransactionType type,
            string description,
            DateTimeOffset transactionDate,
            string? category = null,
            string? notes = null)
            : base()
        {
            UserId = userId;
            Value = value ?? throw new ArgumentNullException(nameof(value));
            Type = type;
            Description = description ?? string.Empty;
            TransactionDate = transactionDate;
            Category = category;
            Notes = notes;
        }

        /// <summary>
        /// Factory method para criar uma nova transação financeira com validações completas.
        /// </summary>
        public static FinancialTransaction Create(
            Guid userId,
            Money value,
            TransactionType type,
            string description,
            DateTimeOffset? transactionDate = null,
            string? category = null,
            string? notes = null)
        {
            // Validações de domínio
            if (userId == Guid.Empty)
                throw new DomainException("INVALID_USER", "UserId is required");

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (value.Amount < 0)
                throw new DomainException("INVALID_AMOUNT", "Transaction value must be non-negative");

            if (string.IsNullOrWhiteSpace(description))
                throw new DomainException("INVALID_DESCRIPTION", "Description is required");

            if (description.Length > 500)
                throw new DomainException("INVALID_DESCRIPTION", "Description cannot exceed 500 characters");

            var txDate = transactionDate ?? DateTimeOffset.UtcNow;

            if (txDate > DateTimeOffset.UtcNow.AddDays(1))
                throw new DomainException("INVALID_DATE", "Transaction date cannot be in the future");

            var transaction = new FinancialTransaction(userId, value, type, description, txDate, category, notes);

            // Domain Event
            transaction.AddDomainEvent(new TransactionCreatedEvent(transaction));

            return transaction;
        }

        /// <summary>
        /// Atualiza a descrição da transação.
        /// </summary>
        public void UpdateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new DomainException("INVALID_DESCRIPTION", "Description is required");

            if (description.Length > 500)
                throw new DomainException("INVALID_DESCRIPTION", "Description cannot exceed 500 characters");

            Description = description;
        }

        /// <summary>
        /// Atualiza o valor da transação.
        /// </summary>
        public void UpdateValue(Money value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (value.Amount < 0)
                throw new DomainException("INVALID_AMOUNT", "Transaction value must be non-negative");

            if (value.Currency != Value.Currency)
                throw new DomainException("INVALID_CURRENCY", "Cannot change transaction currency");

            Value = value;
        }

        /// <summary>
        /// Atualiza a data da transação.
        /// </summary>
        public void UpdateTransactionDate(DateTimeOffset transactionDate)
        {
            if (transactionDate > DateTimeOffset.UtcNow.AddDays(1))
                throw new DomainException("INVALID_DATE", "Transaction date cannot be in the future");

            TransactionDate = transactionDate;
        }

        /// <summary>
        /// Atualiza a categoria da transação.
        /// </summary>
        public void UpdateCategory(string? category)
        {
            Category = category;
        }

        /// <summary>
        /// Atualiza as notas da transação.
        /// </summary>
        public void UpdateNotes(string? notes)
        {
            Notes = notes;
        }

        /// <summary>
        /// Verifica se é uma receita.
        /// </summary>
        public bool IsRevenue() => Type == TransactionType.Revenue;

        /// <summary>
        /// Verifica se é uma despesa.
        /// </summary>
        public bool IsExpense() => Type == TransactionType.Expense;
    }
}

