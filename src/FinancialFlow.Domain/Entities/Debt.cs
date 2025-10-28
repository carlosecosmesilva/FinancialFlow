using System;
using FinancialFlow.Domain.Enums;
using FinancialFlow.Domain.ValueObjects;

namespace FinancialFlow.Domain.Entities
{
    /// <summary>
    /// Aggregate Root que representa uma dívida.
    /// Baseado na planilha "Planilha para organizar dividas 2024.xlsx"
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
                throw new ArgumentException("UserId is required.", nameof(userId));

            if (string.IsNullOrWhiteSpace(creditor))
                throw new ArgumentException("Creditor name is required.", nameof(creditor));

            if (installmentAmount is null)
                throw new ArgumentNullException(nameof(installmentAmount));

            if (initialAmount is null)
                throw new ArgumentNullException(nameof(initialAmount));

            if (totalInstallments <= 0)
                throw new ArgumentOutOfRangeException(nameof(totalInstallments), "Total installments must be greater than zero.");

            if (interestRate < 0)
                throw new ArgumentOutOfRangeException(nameof(interestRate), "Interest rate cannot be negative.");

            var debt = new Debt(userId, creditor, installmentAmount, totalInstallments, interestRate, initialAmount, priority, nextDueDate, description);

            // Adicionar Domain Event se necessário
            // debt.AddDomainEvent(new DebtCreatedEvent(debt));

            return debt;
        }

        /// <summary>
        /// Registra o pagamento de uma parcela.
        /// </summary>
        public void RegisterPayment(DateTimeOffset paymentDate)
        {
            if (Status == DebtStatus.Paid)
                throw new InvalidOperationException("Debt is already paid off.");

            PaidInstallments++;

            if (PaidInstallments >= TotalInstallments)
            {
                Status = DebtStatus.Paid;
                NextDueDate = null;
                // AddDomainEvent(new DebtPaidOffEvent(this));
            }
            else if (NextDueDate.HasValue)
            {
                // Próximo vencimento é após 1 mês
                NextDueDate = NextDueDate.Value.AddMonths(1);
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
