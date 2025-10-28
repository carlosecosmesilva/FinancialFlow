namespace FinancialFlow.Domain.Enums
{
    /// <summary>
    /// Categorias de transações baseadas em planilhas de controle financeiro.
    /// </summary>
    public enum CategoryType
    {
        // === DESPESAS ===
        /// <summary>
        /// Moradia (aluguel, condomínio, IPTU)
        /// </summary>
        Housing = 0,

        /// <summary>
        /// Transporte (combustível, transporte público, manutenção)
        /// </summary>
        Transportation = 1,

        /// <summary>
        /// Alimentação (supermercado, restaurantes)
        /// </summary>
        Food = 2,

        /// <summary>
        /// Saúde (plano de saúde, medicamentos, consultas)
        /// </summary>
        Health = 3,

        /// <summary>
        /// Educação (cursos, livros, mensalidade escolar)
        /// </summary>
        Education = 4,

        /// <summary>
        /// Lazer (entretenimento, viagens, hobbies)
        /// </summary>
        Leisure = 5,

        /// <summary>
        /// Contas e serviços (água, luz, internet, telefone)
        /// </summary>
        Bills = 6,

        /// <summary>
        /// Vestuário (roupas, calçados)
        /// </summary>
        Clothing = 7,

        /// <summary>
        /// Despesas extras não categorizadas
        /// </summary>
        Extra = 8,

        // === RECEITAS ===
        /// <summary>
        /// Salário fixo
        /// </summary>
        Salary = 100,

        /// <summary>
        /// Horas extras
        /// </summary>
        Overtime = 101,

        /// <summary>
        /// Bônus e comissões
        /// </summary>
        Bonus = 102,

        /// <summary>
        /// Outras receitas
        /// </summary>
        OtherIncome = 103,

        // === INVESTIMENTOS ===
        /// <summary>
        /// Investimentos em geral
        /// </summary>
        Investments = 200,

        /// <summary>
        /// Categoria não especificada
        /// </summary>
        Uncategorized = 999
    }
}
