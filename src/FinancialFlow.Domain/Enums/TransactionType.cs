namespace FinancialFlow.Domain.Enums
{
    /// <summary>
    /// Define o tipo de transação financeira.
    /// </summary>
    public enum TransactionType
    {
        /// <summary>
        /// Receita - entrada de dinheiro
        /// </summary>
        Revenue = 0,

        /// <summary>
        /// Despesa - saída de dinheiro
        /// </summary>
        Expense = 1
    }
}
