using System;

namespace FinancialFlow.Domain.Exceptions
{
    /// <summary>
    /// Exceção base para todas as violações de regras de negócio do domínio.
    /// </summary>
    public class DomainException : Exception
    {
        public string Code { get; }

        public DomainException(string message) : base(message)
        {
            Code = "DOMAIN_ERROR";
        }

        public DomainException(string code, string message) : base(message)
        {
            Code = code;
        }

        public DomainException(string message, Exception innerException)
            : base(message, innerException)
        {
            Code = "DOMAIN_ERROR";
        }
    }
}
