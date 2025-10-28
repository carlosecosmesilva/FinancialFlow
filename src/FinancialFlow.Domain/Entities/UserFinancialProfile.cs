using System;
using FinancialFlow.Domain.ValueObjects;

namespace FinancialFlow.Domain.Entities
{
    /// <summary>
    /// Aggregate Root que representa o perfil financeiro de um usuário.
    /// Consolida informações gerais sobre a situação financeira.
    /// </summary>
    public class UserFinancialProfile : Entity
    {
        public Guid UserId { get; private set; }

        /// <summary>
        /// Nome do usuário
        /// </summary>
        public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// Email do usuário
        /// </summary>
        public Email Email { get; private set; } = null!;

        /// <summary>
        /// Renda mensal do usuário
        /// </summary>
        public Money? MonthlyIncome { get; private set; }

        /// <summary>
        /// Meta de economia mensal
        /// </summary>
        public Money? MonthlySavingsGoal { get; private set; }

        /// <summary>
        /// Moeda padrão utilizada pelo usuário
        /// </summary>
        public string DefaultCurrency { get; private set; } = "BRL";

        /// <summary>
        /// Data da última atualização do perfil
        /// </summary>
        public DateTimeOffset? LastUpdatedAt { get; private set; }

        // Construtor protegido para EF Core
        protected UserFinancialProfile() { }

        private UserFinancialProfile(
            Guid userId,
            string name,
            Email email,
            string defaultCurrency = "BRL",
            Money? monthlyIncome = null,
            Money? monthlySavingsGoal = null)
            : base()
        {
            UserId = userId;
            Name = name;
            Email = email;
            DefaultCurrency = defaultCurrency;
            MonthlyIncome = monthlyIncome;
            MonthlySavingsGoal = monthlySavingsGoal;
            LastUpdatedAt = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// Factory method para criar um novo perfil financeiro.
        /// </summary>
        public static UserFinancialProfile Create(
            Guid userId,
            string name,
            Email email,
            string defaultCurrency = "BRL",
            Money? monthlyIncome = null,
            Money? monthlySavingsGoal = null)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId is required.", nameof(userId));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));

            if (email is null)
                throw new ArgumentNullException(nameof(email));

            var profile = new UserFinancialProfile(userId, name, email, defaultCurrency, monthlyIncome, monthlySavingsGoal);

            // AddDomainEvent(new UserFinancialProfileCreatedEvent(profile));

            return profile;
        }

        /// <summary>
        /// Atualiza a renda mensal.
        /// </summary>
        public void UpdateMonthlyIncome(Money income)
        {
            if (income is null)
                throw new ArgumentNullException(nameof(income));

            MonthlyIncome = income;
            LastUpdatedAt = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// Define a meta de economia mensal.
        /// </summary>
        public void SetSavingsGoal(Money goal)
        {
            if (goal is null)
                throw new ArgumentNullException(nameof(goal));

            MonthlySavingsGoal = goal;
            LastUpdatedAt = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// Atualiza o nome do usuário.
        /// </summary>
        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));

            Name = name;
            LastUpdatedAt = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// Atualiza o email do usuário.
        /// </summary>
        public void UpdateEmail(Email email)
        {
            if (email is null)
                throw new ArgumentNullException(nameof(email));

            Email = email;
            LastUpdatedAt = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// Atualiza a moeda padrão.
        /// </summary>
        public void UpdateDefaultCurrency(string currency)
        {
            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Currency is required.", nameof(currency));

            DefaultCurrency = currency.Trim().ToUpperInvariant();
            LastUpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}
