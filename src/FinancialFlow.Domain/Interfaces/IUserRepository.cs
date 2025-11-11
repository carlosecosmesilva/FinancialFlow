using System;
using System.Threading;
using System.Threading.Tasks;
using FinancialFlow.Domain.Entities;
using FinancialFlow.Domain.ValueObjects;

namespace FinancialFlow.Domain.Interfaces
{
    /// <summary>
    /// Repositório do agregado User.
    /// Operações focadas no agregado, queries complexas vão para Application layer.
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Busca um usuário pelo email (Value Object).
        /// </summary>
        Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Verifica se um email já está registrado.
        /// </summary>
        Task<bool> EmailExistsAsync(Email email, CancellationToken cancellationToken = default);
    }
}