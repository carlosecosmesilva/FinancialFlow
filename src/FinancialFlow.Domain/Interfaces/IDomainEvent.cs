using System;

namespace FinancialFlow.Domain.Interfaces
{
    /// <summary>
    /// Marca um evento de domínio. Implementações podem adicionar metadados adicionais.
    /// </summary>
    public interface IDomainEvent
    {
        DateTimeOffset OccurredOn { get; }
    }
}