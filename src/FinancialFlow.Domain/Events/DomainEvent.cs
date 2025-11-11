using System;
using FinancialFlow.Domain.Interfaces;

namespace FinancialFlow.Domain.Events
{
    /// <summary>
    /// Classe base abstrata para Domain Events.
    /// </summary>
    public abstract record DomainEvent : IDomainEvent
    {
        public Guid EventId { get; init; }
        public DateTimeOffset OccurredOn { get; init; }

        protected DomainEvent()
        {
            EventId = Guid.NewGuid();
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}
