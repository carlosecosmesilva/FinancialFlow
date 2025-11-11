using System;
using FinancialFlow.Domain.ValueObjects;

namespace FinancialFlow.Domain.Events
{
    /// <summary>
    /// Evento disparado quando o valor de um investimento é atualizado.
    /// </summary>
    public sealed record InvestmentValueUpdatedEvent : DomainEvent
    {
        public Guid InvestmentId { get; init; }
        public Guid UserId { get; init; }
        public Money PreviousValue { get; init; }
        public Money NewValue { get; init; }
        public decimal ReturnPercentage { get; init; }

        public InvestmentValueUpdatedEvent(
            Guid investmentId,
            Guid userId,
            Money previousValue,
            Money newValue)
        {
            InvestmentId = investmentId;
            UserId = userId;
            PreviousValue = previousValue;
            NewValue = newValue;
            ReturnPercentage = previousValue.Amount == 0
                ? 0
                : ((newValue.Amount - previousValue.Amount) / previousValue.Amount) * 100;
        }
    }
}
