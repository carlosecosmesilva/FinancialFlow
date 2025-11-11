using System;

namespace FinancialFlow.Domain.ValueObjects
{
    /// <summary>
    /// Value Object que representa um período (mês/ano).
    /// </summary>
    public sealed record Period
    {
        public int Month { get; init; }
        public int Year { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }

        public Period(int month, int year)
        {
            if (month < 1 || month > 12)
                throw new ArgumentException("Month must be between 1 and 12", nameof(month));

            if (year < 2000 || year > 2100)
                throw new ArgumentException("Year must be between 2000 and 2100", nameof(year));

            Month = month;
            Year = year;
            StartDate = new DateTime(year, month, 1);
            EndDate = StartDate.AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// Retorna o período atual (mês/ano corrente).
        /// </summary>
        public static Period Current() => new(DateTime.UtcNow.Month, DateTime.UtcNow.Year);

        /// <summary>
        /// Retorna o próximo mês.
        /// </summary>
        public Period NextMonth() =>
            Month == 12
                ? new Period(1, Year + 1)
                : new Period(Month + 1, Year);

        /// <summary>
        /// Retorna o mês anterior.
        /// </summary>
        public Period PreviousMonth() =>
            Month == 1
                ? new Period(12, Year - 1)
                : new Period(Month - 1, Year);

        /// <summary>
        /// Verifica se uma data pertence a este período.
        /// </summary>
        public bool Contains(DateTime date) => date >= StartDate && date <= EndDate;

        /// <summary>
        /// Representação textual do período.
        /// </summary>
        public override string ToString() => $"{Month:00}/{Year}";
    }
}
