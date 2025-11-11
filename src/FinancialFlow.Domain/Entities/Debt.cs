using System;
using FinancialFlow.Domain.Enums;
using FinancialFlow.Domain.Events;
using FinancialFlow.Domain.Exceptions;
using FinancialFlow.Domain.ValueObjects;

namespace FinancialFlow.Domain.Entities
{
    /// <summary>
    /// Aggregate Root que representa uma dívida.
    /// </summary>
    public class Debt : Entity
    {
        public Guid UserId { get; private set; }

        /// <summary>
        /// Nome do credor (banco, loja, pessoa, etc)
        /// </summary>
        public string Creditor { get; private set; } = string.Empty;

        /// <summary>
        /// Valor da parcela mensal
        /// </summary>
        public Money InstallmentAmount { get; private set; } = null!;

        /// <summary>
        /// Número total de parcelas
        /// </summary>
        public int TotalInstallments { get; private set; }

        /// <summary>
        /// Número de parcelas já pagas
        /// </summary>
        public int PaidInstallments { get; private set; }

        /// <summary>
        /// Taxa de juros mensal (percentual)
        /// </summary>
        public decimal InterestRate { get; private set; }

        /// <summary>
        /// Valor inicial da dívida
        /// </summary>
        public Money InitialAmount { get; private set; } = null!;

        /// <summary>
        /// Data de vencimento da próxima parcela
        /// </summary>
        public DateTimeOffset? NextDueDate { get; private set; }

        /// <summary>
        /// Status atual da dívida
        /// </summary>
        public DebtStatus Status { get; private set; }

        /// <summary>
        /// Prioridade de pagamento ("Pode esperar?")
        /// </summary>
        public DebtPriority Priority { get; private set; }

        /// <summary>
        /// Descrição ou observações sobre a dívida
        /// </summary>
        public string? Description { get; private set; }

        // Construtor protegido para EF Core
        protected Debt() { }

        private Debt(
            Guid userId,
            string creditor,
            Money installmentAmount,
            int totalInstallments,
            decimal interestRate,
            Money initialAmount,
            DebtPriority priority,
            DateTimeOffset? nextDueDate = null,
            string? description = null)
            : base()
        {
            UserId = userId;
            Creditor = creditor;
            InstallmentAmount = installmentAmount;
            TotalInstallments = totalInstallments;
            PaidInstallments = 0;
            InterestRate = interestRate;
            InitialAmount = initialAmount;
            NextDueDate = nextDueDate;
            Status = DebtStatus.Active;
            Priority = priority;
            Description = description;
        }

        /// <summary>
        /// Factory method para criar uma nova dívida com validações.
        /// </summary>
        public static Debt Create(
            Guid userId,
            string creditor,
            Money installmentAmount,
            int totalInstallments,
            decimal interestRate,
            Money initialAmount,
            DebtPriority priority,
            DateTimeOffset? nextDueDate = null,
            string? description = null)
        {
            if (userId == Guid.Empty)
                throw new DomainException("INVALID_USER", "UserId is required");

            if (string.IsNullOrWhiteSpace(creditor))
                throw new DomainException("INVALID_CREDITOR", "Creditor name is required");

            if (creditor.Length > 200)
                throw new DomainException("INVALID_CREDITOR", "Creditor name cannot exceed 200 characters");

            if (installmentAmount is null)
                throw new ArgumentNullException(nameof(installmentAmount));

            if (installmentAmount.Amount <= 0)
                throw new DomainException("INVALID_AMOUNT", "Installment amount must be greater than zero");

            if (initialAmount is null)
                throw new ArgumentNullException(nameof(initialAmount));

            if (initialAmount.Amount <= 0)
                throw new DomainException("INVALID_AMOUNT", "Initial amount must be greater than zero");

            if (totalInstallments <= 0)
                throw new DomainException("INVALID_INSTALLMENTS", "Total installments must be greater than zero");

            if (totalInstallments > 360)
                throw new DomainException("INVALID_INSTALLMENTS", "Total installments cannot exceed 360 (30 years)");

            if (interestRate < 0)
                throw new DomainException("INVALID_INTEREST", "Interest rate cannot be negative");

            if (interestRate > 100)
                throw new DomainException("INVALID_INTEREST", "Interest rate cannot exceed 100%");

            var debt = new Debt(userId, creditor, installmentAmount, totalInstallments, interestRate, initialAmount, priority, nextDueDate, description);

            return debt;
        }

        /// <summary>
        /// Registra o pagamento de uma parcela.
        /// </summary>
        public void RegisterPayment(DateTimeOffset paymentDate)
        {
            if (Status == DebtStatus.Paid)
                throw new DomainException("DEBT_ALREADY_PAID", "Debt is already paid off");

            PaidInstallments++;

            // Domain Event para parcela paga
            AddDomainEvent(new DebtInstallmentPaidEvent(
                Id,
                UserId,
                InstallmentAmount,
                PaidInstallments,
                TotalInstallments - PaidInstallments));

            if (PaidInstallments >= TotalInstallments)
            {
                Status = DebtStatus.Paid;
                NextDueDate = null;

                // Domain Event para dívida quitada
                AddDomainEvent(new DebtFullyPaidEvent(
                    Id,
                    UserId,
                    Creditor,
                    new Money(InstallmentAmount.Amount * TotalInstallments, InstallmentAmount.Currency)));
            }
            else if (NextDueDate.HasValue)
            {
                // Próximo vencimento é após 1 mês
                NextDueDate = NextDueDate.Value.AddMonths(1);
                Status = DebtStatus.Active; // Remove status de vencido se estava
            }
        }

        /// <summary>
        /// Marca a dívida como vencida.
        /// </summary>
        public void MarkAsOverdue()
        {
            if (Status == DebtStatus.Active)
            {
                Status = DebtStatus.Overdue;
                // AddDomainEvent(new DebtOverdueEvent(this));
            }
        }

        /// <summary>
        /// Renegociar a dívida com novos valores.
        /// </summary>
        public void Renegotiate(Money newInstallmentAmount, int newTotalInstallments, decimal newInterestRate)
        {
            if (newInstallmentAmount is null)
                throw new ArgumentNullException(nameof(newInstallmentAmount));

            if (newTotalInstallments <= 0)
                throw new ArgumentOutOfRangeException(nameof(newTotalInstallments));

            InstallmentAmount = newInstallmentAmount;
            TotalInstallments = newTotalInstallments;
            PaidInstallments = 0;
            InterestRate = newInterestRate;
            Status = DebtStatus.Renegotiated;

            // AddDomainEvent(new DebtRenegotiatedEvent(this));
        }

        /// <summary>
        /// Calcula o valor total restante da dívida.
        /// </summary>
        public Money CalculateRemainingAmount()
        {
            var remainingInstallments = TotalInstallments - PaidInstallments;
            var remainingValue = InstallmentAmount.Amount * remainingInstallments;
            return new Money(remainingValue, InstallmentAmount.Currency);
        }

        /// <summary>
        /// Atualiza a prioridade de pagamento.
        /// </summary>
        public void UpdatePriority(DebtPriority newPriority)
        {
            Priority = newPriority;
        }
    }
}
