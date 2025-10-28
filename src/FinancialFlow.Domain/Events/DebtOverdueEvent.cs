using System;

namespace FinancialFlow.Domain.Events
{
    /// <summary>
    /// Evento disparado quando uma dívida está vencida.
    /// </summary>
    public class DebtOverdueEvent : DomainEventBase
    {
        public Guid DebtId { get; }
        public Guid UserId { get; }
        public string Creditor { get; }
        public DateTimeOffset DueDate { get; }
        public decimal InstallmentAmount { get; }

        public DebtOverdueEvent(
            Guid debtId,
            Guid userId,
            string creditor,
            DateTimeOffset dueDate,
            decimal installmentAmount)
        {
            DebtId = debtId;
            UserId = userId;
            Creditor = creditor;
            DueDate = dueDate;
            InstallmentAmount = installmentAmount;
        }
    }
}
