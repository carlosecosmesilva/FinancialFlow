using System;
using FinancialFlow.Domain.Enums;
using FinancialFlow.Domain.Events;
using FinancialFlow.Domain.Exceptions;
using FinancialFlow.Domain.ValueObjects;

namespace FinancialFlow.Domain.Entities
{
    /// <summary>
    /// Aggregate Root que representa um investimento.
    /// Ações, Tesouro Direto, Renda Fixa, Previdência.
    /// </summary>
    public class Investment : Entity
    {
        public Guid UserId { get; private set; }

        /// <summary>
        /// Nome ou identificação do investimento
        /// </summary>
        public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// Tipo de investimento
        /// </summary>
        public InvestmentType Type { get; private set; }

        /// <summary>
        /// Valor investido inicialmente
        /// </summary>
        public Money InitialAmount { get; private set; } = null!;

        /// <summary>
        /// Valor atual do investimento
        /// </summary>
        public Money CurrentAmount { get; private set; } = null!;

        /// <summary>
        /// Data do investimento inicial
        /// </summary>
        public DateTimeOffset InvestmentDate { get; private set; }

        /// <summary>
        /// Rentabilidade esperada anual (percentual)
        /// </summary>
        public decimal? ExpectedAnnualReturn { get; private set; }

        /// <summary>
        /// Data de vencimento (se aplicável)
        /// </summary>
        public DateTimeOffset? MaturityDate { get; private set; }

        /// <summary>
        /// Indica se o investimento está ativo
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Instituição financeira onde está o investimento
        /// </summary>
        public string? Institution { get; private set; }

        /// <summary>
        /// Observações ou notas sobre o investimento
        /// </summary>
        public string? Notes { get; private set; }

        // Construtor protegido para EF Core
        protected Investment() { }

        private Investment(
            Guid userId,
            string name,
            InvestmentType type,
            Money initialAmount,
            DateTimeOffset investmentDate,
            decimal? expectedAnnualReturn = null,
            DateTimeOffset? maturityDate = null,
            string? institution = null,
            string? notes = null)
            : base()
        {
            UserId = userId;
            Name = name;
            Type = type;
            InitialAmount = initialAmount;
            CurrentAmount = initialAmount; // Inicialmente igual ao valor investido
            InvestmentDate = investmentDate;
            ExpectedAnnualReturn = expectedAnnualReturn;
            MaturityDate = maturityDate;
            IsActive = true;
            Institution = institution;
            Notes = notes;
        }

        /// <summary>
        /// Factory method para criar um novo investimento com validações.
        /// </summary>
        public static Investment Create(
            Guid userId,
            string name,
            InvestmentType type,
            Money initialAmount,
            DateTimeOffset? investmentDate = null,
            decimal? expectedAnnualReturn = null,
            DateTimeOffset? maturityDate = null,
            string? institution = null,
            string? notes = null)
        {
            if (userId == Guid.Empty)
                throw new DomainException("INVALID_USER", "UserId is required");

            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("INVALID_NAME", "Investment name is required");

            if (name.Length > 200)
                throw new DomainException("INVALID_NAME", "Investment name cannot exceed 200 characters");

            if (initialAmount is null)
                throw new ArgumentNullException(nameof(initialAmount));

            if (initialAmount.Amount <= 0)
                throw new DomainException("INVALID_AMOUNT", "Initial amount must be greater than zero");

            if (expectedAnnualReturn.HasValue && (expectedAnnualReturn.Value < -100 || expectedAnnualReturn.Value > 1000))
                throw new DomainException("INVALID_RETURN", "Expected annual return must be between -100% and 1000%");

            var invDate = investmentDate ?? DateTimeOffset.UtcNow;

            if (maturityDate.HasValue && maturityDate.Value <= invDate)
                throw new DomainException("INVALID_DATE", "Maturity date must be after investment date");

            var investment = new Investment(userId, name, type, initialAmount, invDate, expectedAnnualReturn, maturityDate, institution, notes);

            return investment;
        }

        /// <summary>
        /// Atualiza o valor atual do investimento.
        /// </summary>
        public void UpdateCurrentValue(Money newAmount)
        {
            if (newAmount is null)
                throw new ArgumentNullException(nameof(newAmount));

            if (newAmount.Currency != CurrentAmount.Currency)
                throw new DomainException("INVALID_CURRENCY", "Cannot update with different currency");

            if (newAmount.Amount < 0)
                throw new DomainException("INVALID_AMOUNT", "Investment value cannot be negative");

            var previousValue = CurrentAmount;
            CurrentAmount = newAmount;

            // Domain Event
            AddDomainEvent(new InvestmentValueUpdatedEvent(Id, UserId, previousValue, newAmount));
        }

        /// <summary>
        /// Calcula o retorno do investimento (ROI).
        /// </summary>
        public decimal CalculateROI()
        {
            if (InitialAmount.Amount == 0)
                return 0;

            return ((CurrentAmount.Amount - InitialAmount.Amount) / InitialAmount.Amount) * 100;
        }

        /// <summary>
        /// Calcula o lucro/prejuízo atual.
        /// </summary>
        public Money CalculateProfit()
        {
            return CurrentAmount.Subtract(InitialAmount);
        }

        /// <summary>
        /// Resgata (encerra) o investimento.
        /// </summary>
        public void Redeem(DateTimeOffset redemptionDate)
        {
            if (!IsActive)
                throw new DomainException("INVESTMENT_INACTIVE", "Investment is already inactive");

            IsActive = false;
        }

        /// <summary>
        /// Adiciona mais valor ao investimento (aporte).
        /// </summary>
        public void AddContribution(Money amount, DateTimeOffset contributionDate)
        {
            if (amount is null)
                throw new ArgumentNullException(nameof(amount));

            if (amount.Amount <= 0)
                throw new DomainException("INVALID_AMOUNT", "Contribution amount must be greater than zero");

            if (!IsActive)
                throw new DomainException("INVESTMENT_INACTIVE", "Cannot add contribution to inactive investment");

            if (amount.Currency != InitialAmount.Currency)
                throw new DomainException("INVALID_CURRENCY", "Contribution currency must match investment currency");

            InitialAmount = InitialAmount.Add(amount);
            CurrentAmount = CurrentAmount.Add(amount);
        }

        /// <summary>
        /// Verifica se o investimento atingiu a maturidade.
        /// </summary>
        public bool HasMatured()
        {
            return MaturityDate.HasValue && DateTimeOffset.UtcNow >= MaturityDate.Value;
        }
    }
}
