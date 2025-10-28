using System;
using FinancialFlow.Domain.Interfaces;

namespace FinancialFlow.Domain.Events
{
    /// <summary>
    /// Classe base abstrata para todos os eventos de domínio.
    /// </summary>
    public abstract class DomainEventBase : IDomainEvent
    {
        public DateTimeOffset OccurredOn { get; }

        protected DomainEventBase()
        {
            OccurredOn = DateTimeOffset.UtcNow;
        }
    }
}
