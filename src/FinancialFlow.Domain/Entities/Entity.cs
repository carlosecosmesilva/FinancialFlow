using System;
using System.Collections.Generic;
using FinancialFlow.Domain.Interfaces;

namespace FinancialFlow.Domain.Entities
{
    /// <summary>
    /// Classe base abstrata para todas as entidades do domínio.
    /// Fornece Id, CreatedAt e suporte para Domain Events.
    /// </summary>
    public abstract class Entity
    {
        private readonly List<IDomainEvent> _domainEvents = new();

        public Guid Id { get; protected set; }
        public DateTimeOffset CreatedAt { get; protected set; }

        protected Entity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTimeOffset.UtcNow;
        }

        protected Entity(Guid id)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            CreatedAt = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// Eventos do domínio associados a esta entidade. Somente leitura para a infraestrutura.
        /// </summary>
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        /// <summary>
        /// Adiciona um evento de domínio. Usado internamente pelos Aggregates.
        /// </summary>
        /// <param name="domainEvent">Evento a ser adicionado</param>
        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            if (domainEvent is null)
                throw new ArgumentNullException(nameof(domainEvent));

            _domainEvents.Add(domainEvent);
        }

        /// <summary>
        /// Limpa todos os eventos de domínio. Usado pela infraestrutura após o dispatch.
        /// </summary>
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        /// <summary>
        /// Igualdade baseada no Id da entidade.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is not Entity other)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetType() != other.GetType())
                return false;

            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Entity? left, Entity? right)
        {
            if (left is null && right is null)
                return true;

            if (left is null || right is null)
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(Entity? left, Entity? right)
        {
            return !(left == right);
        }
    }
}
