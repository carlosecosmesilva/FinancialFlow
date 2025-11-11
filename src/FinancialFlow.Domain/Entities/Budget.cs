using System;
using System.Collections.Generic;
using System.Linq;
using FinancialFlow.Domain.Enums;
using FinancialFlow.Domain.Events;
using FinancialFlow.Domain.Exceptions;
using FinancialFlow.Domain.ValueObjects;

namespace FinancialFlow.Domain.Entities
{
    public class Budget : Entity
    {
        private readonly List<BudgetCategory> _categories = new();

        public Guid UserId { get; private set; }
        public string Name { get; private set; }
        public Period Period { get; private set; }
        public Money TotalAmount { get; private set; }
        public BudgetStatus Status { get; private set; }
        public IReadOnlyCollection<BudgetCategory> Categories => _categories.AsReadOnly();

        private Budget() // EF Constructor
        {
            Name = null!;
            Period = null!;
            TotalAmount = null!;
        }

        public Budget(Guid userId, string name, Period period, Money totalAmount)
        {
            if (userId == Guid.Empty)
                throw new DomainException("UserId cannot be empty");

            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Budget name is required");

            UserId = userId;
            Name = name;
            Period = period;
            TotalAmount = totalAmount;
            Status = BudgetStatus.Active;
            CreatedAt = DateTime.UtcNow;

            AddDomainEvent(new BudgetCreatedEvent(this));
        }

        public void AddCategory(string categoryName, Money allocatedAmount)
        {
            if (_categories.Sum(c => c.AllocatedAmount.Amount) + allocatedAmount.Amount > TotalAmount.Amount)
                throw new DomainException("Total allocated exceeds budget amount");

            var category = new BudgetCategory(categoryName, allocatedAmount);
            _categories.Add(category);

            AddDomainEvent(new BudgetCategoryAddedEvent(Id, categoryName, allocatedAmount));
        }

        public void RecordExpense(string categoryName, Money amount)
        {
            var category = _categories.FirstOrDefault(c => c.Name == categoryName)
                ?? throw new DomainException($"Category {categoryName} not found");

            category.AddExpense(amount);

            if (category.IsOverBudget())
                AddDomainEvent(new BudgetExceededEvent(Id, categoryName, category.AllocatedAmount, category.SpentAmount));
        }

        public decimal GetUtilizationPercentage()
            => _categories.Any()
                ? _categories.Sum(c => c.UtilizationPercentage) / _categories.Count
                : 0;

        public void Close()
        {
            if (Status == BudgetStatus.Closed)
                throw new DomainException("Budget is already closed");

            Status = BudgetStatus.Closed;
            AddDomainEvent(new BudgetClosedEvent(Id, GetUtilizationPercentage()));
        }

        /// <summary>
        /// Pausa o orçamento temporariamente.
        /// </summary>
        public void Pause()
        {
            if (Status == BudgetStatus.Closed)
                throw new DomainException("Cannot pause a closed budget");

            Status = BudgetStatus.Paused;
        }

        /// <summary>
        /// Reativa um orçamento pausado.
        /// </summary>
        public void Resume()
        {
            if (Status != BudgetStatus.Paused)
                throw new DomainException("Only paused budgets can be resumed");

            Status = BudgetStatus.Active;
        }

        /// <summary>
        /// Retorna o total alocado em todas as categorias.
        /// </summary>
        public Money GetTotalAllocated()
        {
            if (!_categories.Any())
                return Money.Zero(TotalAmount.Currency);

            var totalAmount = _categories.Sum(c => c.AllocatedAmount.Amount);
            return new Money(totalAmount, TotalAmount.Currency);
        }

        /// <summary>
        /// Retorna o total gasto em todas as categorias.
        /// </summary>
        public Money GetTotalSpent()
        {
            if (!_categories.Any())
                return Money.Zero(TotalAmount.Currency);

            var totalSpent = _categories.Sum(c => c.SpentAmount.Amount);
            return new Money(totalSpent, TotalAmount.Currency);
        }

        /// <summary>
        /// Retorna o saldo restante do orçamento.
        /// </summary>
        public Money GetRemainingBudget()
        {
            var totalSpent = GetTotalSpent();
            return new Money(
                Math.Max(0, TotalAmount.Amount - totalSpent.Amount),
                TotalAmount.Currency);
        }

        /// <summary>
        /// Verifica se o orçamento está excedido.
        /// </summary>
        public bool IsOverBudget() => GetTotalSpent().Amount > TotalAmount.Amount;

        /// <summary>
        /// Factory method para criar um orçamento com validações completas.
        /// </summary>
        public static Budget Create(
            Guid userId,
            string name,
            Period period,
            Money totalAmount)
        {
            if (userId == Guid.Empty)
                throw new DomainException("INVALID_USER", "UserId cannot be empty");

            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("INVALID_NAME", "Budget name is required");

            if (name.Length > 200)
                throw new DomainException("INVALID_NAME", "Budget name cannot exceed 200 characters");

            if (period is null)
                throw new ArgumentNullException(nameof(period));

            if (totalAmount is null)
                throw new ArgumentNullException(nameof(totalAmount));

            if (totalAmount.Amount <= 0)
                throw new DomainException("INVALID_AMOUNT", "Budget amount must be greater than zero");

            return new Budget(userId, name, period, totalAmount);
        }
    }
}