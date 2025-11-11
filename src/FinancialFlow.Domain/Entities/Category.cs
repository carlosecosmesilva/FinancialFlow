using System;
using FinancialFlow.Domain.ValueObjects;

namespace FinancialFlow.Domain.Entities
{
    public class Category : Entity
    {
        public Guid UserId { get; private set; }
        public string Name { get; private set; } = null!;

        protected Category() { }

        private Category(Guid userId, string name) : base()
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Name = name;
            CreatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Método factory para criar uma nova categoria com validações.
        /// </summary>
        public static Category Create(Guid userId, string name)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId is required.", nameof(userId));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));

            return new Category(userId, name);
        }

        /// <summary>
        /// Atualiza o nome da categoria.
        /// </summary>
        /// <param name="name">Novo nome.</param>
        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));

            Name = name;
        }
    }
}