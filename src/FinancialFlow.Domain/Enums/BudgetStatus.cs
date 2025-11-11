namespace FinancialFlow.Domain.Enums
{
    /// <summary>
    /// Status de um orçamento.
    /// </summary>
    public enum BudgetStatus
    {
        /// <summary>
        /// Orçamento ativo e sendo utilizado.
        /// </summary>
        Active = 1,

        /// <summary>
        /// Orçamento pausado temporariamente.
        /// </summary>
        Paused = 2,

        /// <summary>
        /// Orçamento encerrado/fechado.
        /// </summary>
        Closed = 3
    }
}
