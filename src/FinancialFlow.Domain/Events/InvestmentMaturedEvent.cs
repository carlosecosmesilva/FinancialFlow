using System;

namespace FinancialFlow.Domain.Events
{
    /// <summary>
    /// Evento disparado quando um investimento atinge a maturidade.
    /// </summary>
    public class InvestmentMaturedEvent : DomainEventBase
    {
        public Guid InvestmentId { get; }
        public Guid UserId { get; }
        public string InvestmentName { get; }
        public decimal FinalAmount { get; }
        public decimal InitialAmount { get; }
        public decimal Profit { get; }

        public InvestmentMaturedEvent(
            Guid investmentId,
            Guid userId,
            string investmentName,
            decimal finalAmount,
            decimal initialAmount)
        {
            InvestmentId = investmentId;
            UserId = userId;
            InvestmentName = investmentName;
            FinalAmount = finalAmount;
            InitialAmount = initialAmount;
            Profit = finalAmount - initialAmount;
        }
    }
}
