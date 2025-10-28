using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FinancialFlow.Domain.Entities;
using FinancialFlow.Domain.Enums;

namespace FinancialFlow.Domain.Interfaces
{
    /// <summary>
    /// Repositório específico para transações financeiras.
    /// Estende IRepository com operações especializadas.
    /// </summary>
    public interface ITransactionRepository : IRepository<FinancialTransaction>
    {
        /// <summary>
        /// Busca transações de um usuário em um período específico.
        /// </summary>
        Task<IEnumerable<FinancialTransaction>> GetByPeriodAsync(
            Guid userId,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calcula o total de uma categoria específica em um mês/ano.
        /// </summary>
        Task<decimal> GetCategoryTotalAsync(
            Guid userId,
            string category,
            int month,
            int year,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Busca transações por tipo (receita ou despesa).
        /// </summary>
        Task<IEnumerable<FinancialTransaction>> GetByTypeAsync(
            Guid userId,
            TransactionType type,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Busca transações de um usuário com paginação.
        /// </summary>
        Task<IEnumerable<FinancialTransaction>> GetByUserAsync(
            Guid userId,
            int pageNumber = 1,
            int pageSize = 20,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calcula o total de receitas de um usuário em um período.
        /// </summary>
        Task<decimal> GetTotalRevenueAsync(
            Guid userId,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calcula o total de despesas de um usuário em um período.
        /// </summary>
        Task<decimal> GetTotalExpenseAsync(
            Guid userId,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            CancellationToken cancellationToken = default);
    }
}
