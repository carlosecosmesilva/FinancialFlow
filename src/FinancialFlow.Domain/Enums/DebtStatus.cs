namespace FinancialFlow.Domain.Enums
{
    /// <summary>
    /// Status de uma dívida.
    /// </summary>
    public enum DebtStatus
    {
        /// <summary>
        /// Dívida ativa, ainda sendo paga
        /// </summary>
        Active = 0,

        /// <summary>
        /// Dívida totalmente paga
        /// </summary>
        Paid = 1,

        /// <summary>
        /// Dívida em atraso/vencida
        /// </summary>
        Overdue = 2,

        /// <summary>
        /// Dívida renegociada
        /// </summary>
        Renegotiated = 3
    }
}
