using System;
using System.Threading;
using System.Threading.Tasks;
using FinancialFlow.Domain.Entities;

namespace FinancialFlow.Domain.Interfaces
{
    /// <summary>
    /// Repositório para perfil financeiro do usuário.
    /// </summary>
    public interface IUserFinancialProfileRepository : IRepository<UserFinancialProfile>
    {
        /// <summary>
        /// Busca o perfil financeiro de um usuário.
        /// </summary>
        Task<UserFinancialProfile?> GetByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Verifica se um usuário já possui perfil financeiro.
        /// </summary>
        Task<bool> ExistsAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
    }
}
