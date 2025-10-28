using System.Threading;
using System.Threading.Tasks;

namespace FinancialFlow.Domain.Interfaces
{
    /// <summary>
    /// Interface para Unit of Work.
    /// Responsável por coordenar a persistência de múltiplas operações em uma única transação.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Persiste todas as mudanças pendentes no banco de dados.
        /// </summary>
        /// <returns>Número de registros afetados</returns>
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }
}
