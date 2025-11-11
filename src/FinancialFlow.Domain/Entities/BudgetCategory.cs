using System;
using FinancialFlow.Domain.ValueObjects;

namespace FinancialFlow.Domain.Entities
{
    /// <summary>
    /// Entity que representa uma categoria dentro de um orçamento.
    /// Não é um Aggregate Root, pertence ao Budget.
    /// </summary>
    public class BudgetCategory : Entity
    {
        public string Name { get; private set; } = null!;
        public Money AllocatedAmount { get; private set; } = null!;
        public Money SpentAmount { get; private set; } = null!;

        private BudgetCategory() { } // EF Constructor

        public BudgetCategory(string name, Money allocatedAmount)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name is required", nameof(name));

            Name = name;
            AllocatedAmount = allocatedAmount ?? throw new ArgumentNullException(nameof(allocatedAmount));
            SpentAmount = Money.Zero(allocatedAmount.Currency);
        }

        /// <summary>
        /// Adiciona um gasto à categoria.
        /// </summary>
        public void AddExpense(Money amount)
        {
            if (amount is null)
                throw new ArgumentNullException(nameof(amount));

            if (amount.Currency != AllocatedAmount.Currency)
                throw new InvalidOperationException("Currency mismatch");

            SpentAmount = SpentAmount.Add(amount);
        }

        /// <summary>
        /// Verifica se a categoria está acima do orçamento.
        /// </summary>
        public bool IsOverBudget() => SpentAmount.Amount > AllocatedAmount.Amount;

        /// <summary>
        /// Retorna o percentual de utilização do orçamento.
        /// </summary>
        public decimal UtilizationPercentage =>
            AllocatedAmount.Amount == 0
                ? 0
                : (SpentAmount.Amount / AllocatedAmount.Amount) * 100;

        /// <summary>
        /// Retorna o saldo restante da categoria.
        /// </summary>
        public Money RemainingAmount =>
            new Money(
                Math.Max(0, AllocatedAmount.Amount - SpentAmount.Amount),
                AllocatedAmount.Currency);
    }
}
