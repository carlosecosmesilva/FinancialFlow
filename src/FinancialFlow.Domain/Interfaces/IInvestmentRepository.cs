using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FinancialFlow.Domain.Entities;
using FinancialFlow.Domain.Enums;

namespace FinancialFlow.Domain.Interfaces
{
    /// <summary>
    /// Repositório específico para investimentos.
    /// </summary>
    public interface IInvestmentRepository : IRepository<Investment>
    {
        /// <summary>
        /// Busca investimentos ativos de um usuário.
        /// </summary>
        Task<IEnumerable<Investment>> GetActiveInvestmentsAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Busca investimentos por tipo.
        /// </summary>
        Task<IEnumerable<Investment>> GetByTypeAsync(
            Guid userId,
            InvestmentType type,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calcula o valor total investido (ativo).
        /// </summary>
        Task<decimal> GetTotalInvestedAmountAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calcula o valor atual total da carteira.
        /// </summary>
        Task<decimal> GetTotalCurrentValueAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Busca investimentos que atingiram a maturidade.
        /// </summary>
        Task<IEnumerable<Investment>> GetMaturedInvestmentsAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
    }
}
