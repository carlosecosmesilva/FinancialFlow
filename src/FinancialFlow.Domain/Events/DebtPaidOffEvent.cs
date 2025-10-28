using System;

namespace FinancialFlow.Domain.Events
{
    /// <summary>
    /// Evento disparado quando uma dívida é totalmente paga.
    /// </summary>
    public class DebtPaidOffEvent : DomainEventBase
    {
        public Guid DebtId { get; }
        public Guid UserId { get; }
        public string Creditor { get; }
        public decimal TotalPaid { get; }

        public DebtPaidOffEvent(
            Guid debtId,
            Guid userId,
            string creditor,
            decimal totalPaid)
        {
            DebtId = debtId;
            UserId = userId;
            Creditor = creditor;
            TotalPaid = totalPaid;
        }
    }
}
