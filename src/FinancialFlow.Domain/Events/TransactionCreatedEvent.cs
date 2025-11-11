using System;
using FinancialFlow.Domain.Entities;
using FinancialFlow.Domain.Enums;
using FinancialFlow.Domain.ValueObjects;

namespace FinancialFlow.Domain.Events
{
    /// <summary>
    /// Evento disparado quando uma nova transação financeira é criada.
    /// </summary>
    public sealed record TransactionCreatedEvent : DomainEvent
    {
        public Guid TransactionId { get; init; }
        public Guid UserId { get; init; }
        public string Description { get; init; }
        public Money Value { get; init; }
        public TransactionType Type { get; init; }
        public DateTimeOffset TransactionDate { get; init; }

        public TransactionCreatedEvent(FinancialTransaction transaction)
        {
            TransactionId = transaction.Id;
            UserId = transaction.UserId;
            Description = transaction.Description;
            Value = transaction.Value;
            Type = transaction.Type;
            TransactionDate = transaction.TransactionDate;
        }
    }
}
