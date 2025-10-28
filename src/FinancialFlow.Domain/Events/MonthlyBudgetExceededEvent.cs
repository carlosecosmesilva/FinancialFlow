using System;

namespace FinancialFlow.Domain.Events
{
    /// <summary>
    /// Evento disparado quando o orçamento mensal de uma categoria é excedido.
    /// </summary>
    public class MonthlyBudgetExceededEvent : DomainEventBase
    {
        public Guid UserId { get; }
        public string Category { get; }
        public int Month { get; }
        public int Year { get; }
        public decimal BudgetLimit { get; }
        public decimal CurrentAmount { get; }
        public decimal ExceededBy { get; }

        public MonthlyBudgetExceededEvent(
            Guid userId,
            string category,
            int month,
            int year,
            decimal budgetLimit,
            decimal currentAmount)
        {
            UserId = userId;
            Category = category;
            Month = month;
            Year = year;
            BudgetLimit = budgetLimit;
            CurrentAmount = currentAmount;
            ExceededBy = currentAmount - budgetLimit;
        }
    }
}
