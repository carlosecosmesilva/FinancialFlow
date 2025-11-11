using System;
using FinancialFlow.Domain.ValueObjects;

namespace FinancialFlow.Domain.Entities
{
    public class User : Entity
    {
        public string Name { get; private set; } = null!;
        public Email Email { get; private set; } = null!;
        public string PasswordHash { get; private set; } = null!;

        protected User() { }

        private User(string name, Email email, string passwordHash) : base()
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            CreatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Método factory para criar um novo usuário com validações.
        /// </summary>
        public static User Create(string name, Email email, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));

            ArgumentNullException.ThrowIfNull(email);

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("PasswordHash is required.", nameof(passwordHash));

            return new User(name, email, passwordHash);
        }

        /// <summary>
        /// Atualiza o nome do usuário.
        /// </summary>
        /// <param name="name">Novo nome.</param>
        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));

            Name = name;
        }

        /// <summary>
        /// Atualiza o email do usuário.
        /// </summary>
        /// <param name="email">Novo email.</param>
        public void UpdateEmail(Email email)
        {
            ArgumentNullException.ThrowIfNull(email);
            Email = email;
        }

        /// <summary>
        /// Atualiza o hash da senha do usuário.
        /// </summary>
        /// <param name="passwordHash">Novo hash da senha.</param>
        /// <exception cref="ArgumentException">Se o hash da senha for nulo ou vazio.</exception>
        public void UpdatePasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("PasswordHash is required.", nameof(passwordHash));

            PasswordHash = passwordHash;
        }
    }
}