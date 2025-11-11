using System;
using FinancialFlow.Domain.ValueObjects;

namespace FinancialFlow.Domain.Events
{
    /// <summary>
    /// Evento disparado quando uma parcela de dívida é paga.
    /// </summary>
    public sealed record DebtInstallmentPaidEvent : DomainEvent
    {
        public Guid DebtId { get; init; }
        public Guid UserId { get; init; }
        public Money InstallmentAmount { get; init; }
        public int InstallmentNumber { get; init; }
        public int RemainingInstallments { get; init; }

        public DebtInstallmentPaidEvent(
            Guid debtId,
            Guid userId,
            Money installmentAmount,
            int installmentNumber,
            int remainingInstallments)
        {
            DebtId = debtId;
            UserId = userId;
            InstallmentAmount = installmentAmount;
            InstallmentNumber = installmentNumber;
            RemainingInstallments = remainingInstallments;
        }
    }
}
