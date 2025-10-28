namespace FinancialFlow.Domain.Enums
{
    /// <summary>
    /// Prioridade de pagamento de uma dívida.
    /// </summary>
    public enum DebtPriority
    {
        /// <summary>
        /// Alta prioridade - não pode esperar
        /// </summary>
        High = 0,

        /// <summary>
        /// Média prioridade - pode esperar um pouco
        /// </summary>
        Medium = 1,

        /// <summary>
        /// Baixa prioridade - pode esperar
        /// </summary>
        Low = 2
    }
}
