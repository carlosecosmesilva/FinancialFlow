using System;
using FinancialFlow.Domain.ValueObjects;

namespace FinancialFlow.Domain.Entities
{
    public class Frequency : Entity
    {
        public string Name { get; private set; } = null!;

        protected Frequency() { }

        private Frequency(string name) : base()
        {
            Id = Guid.NewGuid();
            Name = name;
            CreatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Método factory para criar uma nova frequência com validações.
        /// </summary>
        public static Frequency Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));

            return new Frequency(name);
        }
    }
}