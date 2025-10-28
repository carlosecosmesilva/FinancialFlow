namespace FinancialFlow.Domain.Enums
{
    /// <summary>
    /// Tipos de investimento baseados na planilha de controle financeiro.
    /// </summary>
    public enum InvestmentType
    {
        /// <summary>
        /// Ações na bolsa de valores
        /// </summary>
        Stocks = 0,

        /// <summary>
        /// Renda fixa (CDB, LCI, LCA, etc)
        /// </summary>
        FixedIncome = 1,

        /// <summary>
        /// Tesouro Direto
        /// </summary>
        Treasury = 2,

        /// <summary>
        /// Previdência Privada
        /// </summary>
        PrivatePension = 3,

        /// <summary>
        /// Fundos de Investimento
        /// </summary>
        InvestmentFunds = 4,

        /// <summary>
        /// Criptomoedas
        /// </summary>
        Cryptocurrency = 5,

        /// <summary>
        /// Outros tipos de investimento
        /// </summary>
        Others = 99
    }
}
