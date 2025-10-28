using System;
using System.Text.RegularExpressions;

namespace FinancialFlow.Domain.ValueObjects
{
    /// <summary>
    /// Value Object imutável que representa um endereço de email.
    /// </summary>
    public sealed record Email
    {
        private static readonly Regex EmailRegex = new(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string Address { get; init; }

        public Email(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Email address is required.", nameof(address));

            var normalizedAddress = address.Trim().ToLowerInvariant();

            if (!EmailRegex.IsMatch(normalizedAddress))
                throw new ArgumentException("Invalid email format.", nameof(address));

            Address = normalizedAddress;
        }

        /// <summary>
        /// Obtém o domínio do email (parte após @).
        /// </summary>
        public string GetDomain()
        {
            var atIndex = Address.IndexOf('@');
            return atIndex >= 0 ? Address.Substring(atIndex + 1) : string.Empty;
        }

        /// <summary>
        /// Obtém o nome de usuário do email (parte antes do @).
        /// </summary>
        public string GetUsername()
        {
            var atIndex = Address.IndexOf('@');
            return atIndex >= 0 ? Address.Substring(0, atIndex) : Address;
        }

        public override string ToString() => Address;

        public static implicit operator string(Email email) => email.Address;
    }
}
