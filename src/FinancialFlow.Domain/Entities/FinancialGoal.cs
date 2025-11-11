using System;
using FinancialFlow.Domain.ValueObjects;

namespace FinancialFlow.Domain.Entities
{
    public class FinancialGoal : Entity
    {
        public string Name { get; private set; } = null!;
        public decimal TargetAmount { get; private set; }
        public decimal CurrentAmount { get; private set; }
        public DateTime TargetDate { get; private set; }
        public Guid UserId { get; private set; }

        protected FinancialGoal() { }

        private FinancialGoal(string name, decimal targetAmount, DateTime targetDate, Guid userId) : base()
        {
            Id = Guid.NewGuid();
            Name = name;
            TargetAmount = targetAmount;
            CurrentAmount = 0;
            TargetDate = targetDate;
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Método factory para criar uma nova meta financeira com validações.
        /// </summary>
        /// <param name="name">Nome da meta.</param>
        /// <param name="targetAmount">Valor alvo da meta.</param>
        /// <param name="targetDate">Data alvo para alcançar a meta.</param>
        /// <param name="userId">ID do usuário dono da meta.</param>
        /// <returns>Nova instância de FinancialGoal.</returns>
        public static FinancialGoal Create(string name, decimal targetAmount, DateTime targetDate, Guid userId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));

            if (targetAmount <= 0)
                throw new ArgumentException("TargetAmount must be greater than zero.", nameof(targetAmount));

            if (targetDate <= DateTime.UtcNow)
                throw new ArgumentException("TargetDate must be a future date.", nameof(targetDate));

            if (userId == Guid.Empty)
                throw new ArgumentException("UserId is required.", nameof(userId));

            return new FinancialGoal(name, targetAmount, targetDate, userId);
        }

        /// <summary>
        /// Atualiza o valor atual da meta financeira.
        /// </summary>
        /// <param name="amount">Novo valor atual.</param>
        /// <exception cref="ArgumentException">Lançada se o valor for negativo ou exceder o valor alvo.</exception>
        public void UpdateCurrentAmount(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException("CurrentAmount cannot be negative.", nameof(amount));

            if (amount > TargetAmount)
                throw new ArgumentException("CurrentAmount cannot exceed TargetAmount.", nameof(amount));

            CurrentAmount = amount;
        }
    }
}