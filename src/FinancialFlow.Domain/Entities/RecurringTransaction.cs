using System;
using FinancialFlow.Domain.ValueObjects;

namespace FinancialFlow.Domain.Entities
{
    public class RecurringTransaction : Entity
    {
        public Guid UserId { get; private set; }
        public decimal Amount { get; private set; }
        public string Description { get; private set; } = null!;
        public Frequency Frequency { get; private set; }

        protected RecurringTransaction()
        {
            Frequency = default!;
        }

        private RecurringTransaction(Guid userId, decimal amount, string description, Frequency frequency) : base()
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Amount = amount;
            Description = description;
            Frequency = frequency;
            CreatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Método factory para criar uma nova transação recorrente com validações.
        /// </summary>
        public static RecurringTransaction Create(Guid userId, decimal amount, string description, Frequency frequency)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId is required.", nameof(userId));

            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description is required.", nameof(description));

            return new RecurringTransaction(userId, amount, description, frequency);
        }

        /// <summary>
        /// Atualiza o valor da transação recorrente.
        /// </summary>
        /// <param name="amount">Novo valor.</param>
        public void UpdateAmount(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

            Amount = amount;
        }
    }
}