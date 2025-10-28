using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FinancialFlow.Domain.Entities;
using FinancialFlow.Domain.Enums;

namespace FinancialFlow.Domain.Interfaces
{
    /// <summary>
    /// Repositório específico para dívidas.
    /// </summary>
    public interface IDebtRepository : IRepository<Debt>
    {
        /// <summary>
        /// Busca dívidas ativas de um usuário.
        /// </summary>
        Task<IEnumerable<Debt>> GetActiveDebtsAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Busca dívidas por status.
        /// </summary>
        Task<IEnumerable<Debt>> GetByStatusAsync(
            Guid userId,
            DebtStatus status,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Busca dívidas vencidas de um usuário.
        /// </summary>
        Task<IEnumerable<Debt>> GetOverdueDebtsAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Busca dívidas por prioridade.
        /// </summary>
        Task<IEnumerable<Debt>> GetByPriorityAsync(
            Guid userId,
            DebtPriority priority,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calcula o valor total de dívidas ativas.
        /// </summary>
        Task<decimal> GetTotalActiveDebtAmountAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
    }
}
