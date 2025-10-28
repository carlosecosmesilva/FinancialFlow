using System;

namespace FinancialFlow.Domain.Events
{
    /// <summary>
    /// Evento disparado quando uma nova transação é registrada.
    /// </summary>
    public class TransactionRegisteredEvent : DomainEventBase
    {
        public Guid TransactionId { get; }
        public Guid UserId { get; }
        public decimal Amount { get; }
        public string Currency { get; }
        public string TransactionType { get; }

        public TransactionRegisteredEvent(
            Guid transactionId,
            Guid userId,
            decimal amount,
            string currency,
            string transactionType)
        {
            TransactionId = transactionId;
            UserId = userId;
            Amount = amount;
            Currency = currency;
            TransactionType = transactionType;
        }
    }
}
