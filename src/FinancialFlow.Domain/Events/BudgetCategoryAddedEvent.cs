using System;
using FinancialFlow.Domain.ValueObjects;

namespace FinancialFlow.Domain.Events
{
    /// <summary>
    /// Evento disparado quando uma categoria é adicionada a um orçamento.
    /// </summary>
    public sealed record BudgetCategoryAddedEvent : DomainEvent
    {
        public Guid BudgetId { get; init; }
        public string CategoryName { get; init; }
        public Money AllocatedAmount { get; init; }

        public BudgetCategoryAddedEvent(Guid budgetId, string categoryName, Money allocatedAmount)
        {
            BudgetId = budgetId;
            CategoryName = categoryName;
            AllocatedAmount = allocatedAmount;
        }
    }
}
