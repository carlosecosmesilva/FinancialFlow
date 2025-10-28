using System;
using System.Threading;
using System.Threading.Tasks;
using FinancialFlow.Domain.Entities;

namespace FinancialFlow.Domain.Interfaces
{
    /// <summary>
    /// Interface genérica para repositórios de entidades do domínio.
    /// </summary>
    /// <typeparam name="T">Tipo da entidade que herda de Entity</typeparam>
    public interface IRepository<T> where T : Entity
    {
        /// <summary>
        /// Adiciona uma nova entidade ao repositório.
        /// </summary>
        Task AddAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Atualiza uma entidade existente.
        /// </summary>
        void Update(T entity);

        /// <summary>
        /// Busca uma entidade por Id.
        /// </summary>
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
