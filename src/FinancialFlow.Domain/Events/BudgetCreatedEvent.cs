using System;
using FinancialFlow.Domain.Entities;
using FinancialFlow.Domain.ValueObjects;

namespace FinancialFlow.Domain.Events
{
    /// <summary>
    /// Evento disparado quando um novo orçamento é criado.
    /// </summary>
    public sealed record BudgetCreatedEvent : DomainEvent
    {
        public Guid BudgetId { get; init; }
        public Guid UserId { get; init; }
        public string Name { get; init; }
        public Period Period { get; init; }
        public Money TotalAmount { get; init; }

        public BudgetCreatedEvent(Budget budget)
        {
            BudgetId = budget.Id;
            UserId = budget.UserId;
            Name = budget.Name;
            Period = budget.Period;
            TotalAmount = budget.TotalAmount;
        }
    }
}
