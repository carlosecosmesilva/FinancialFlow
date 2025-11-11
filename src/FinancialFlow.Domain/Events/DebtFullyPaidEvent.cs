using System;
using FinancialFlow.Domain.ValueObjects;

namespace FinancialFlow.Domain.Events
{
    /// <summary>
    /// Evento disparado quando uma dívida é totalmente quitada.
    /// </summary>
    public sealed record DebtFullyPaidEvent : DomainEvent
    {
        public Guid DebtId { get; init; }
        public Guid UserId { get; init; }
        public string Creditor { get; init; }
        public Money TotalPaid { get; init; }

        public DebtFullyPaidEvent(Guid debtId, Guid userId, string creditor, Money totalPaid)
        {
            DebtId = debtId;
            UserId = userId;
            Creditor = creditor;
            TotalPaid = totalPaid;
        }
    }
}
