using System;
using FinancialFlow.Domain.ValueObjects;

namespace FinancialFlow.Domain.Events
{
    /// <summary>
    /// Evento disparado quando o gasto de uma categoria excede o orçamento alocado.
    /// </summary>
    public sealed record BudgetExceededEvent : DomainEvent
    {
        public Guid BudgetId { get; init; }
        public string CategoryName { get; init; }
        public Money AllocatedAmount { get; init; }
        public Money SpentAmount { get; init; }
        public decimal ExceededPercentage { get; init; }

        public BudgetExceededEvent(Guid budgetId, string categoryName, Money allocated, Money spent)
        {
            BudgetId = budgetId;
            CategoryName = categoryName;
            AllocatedAmount = allocated;
            SpentAmount = spent;
            ExceededPercentage = allocated.Amount == 0
                ? 0
                : ((spent.Amount - allocated.Amount) / allocated.Amount) * 100;
        }
    }
}
