using System;

namespace FinancialFlow.Domain.Events
{
    /// <summary>
    /// Evento disparado quando um orçamento é fechado.
    /// </summary>
    public sealed record BudgetClosedEvent : DomainEvent
    {
        public Guid BudgetId { get; init; }
        public decimal UtilizationPercentage { get; init; }

        public BudgetClosedEvent(Guid budgetId, decimal utilizationPercentage)
        {
            BudgetId = budgetId;
            UtilizationPercentage = utilizationPercentage;
        }
    }
}
