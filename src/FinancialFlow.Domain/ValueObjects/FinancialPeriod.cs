using System;

namespace FinancialFlow.Domain.ValueObjects
{
    /// <summary>
    /// Value Object imutável que representa um período financeiro (mês/ano).
    /// </summary>
    public sealed record FinancialPeriod
    {
        public int Month { get; init; }
        public int Year { get; init; }

        public FinancialPeriod(int month, int year)
        {
            if (month < 1 || month > 12)
                throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 1 and 12.");

            if (year < 1900 || year > 2100)
                throw new ArgumentOutOfRangeException(nameof(year), "Year must be between 1900 and 2100.");

            Month = month;
            Year = year;
        }

        /// <summary>
        /// Cria um período para o mês/ano atual.
        /// </summary>
        public static FinancialPeriod Current()
        {
            var now = DateTimeOffset.UtcNow;
            return new FinancialPeriod(now.Month, now.Year);
        }

        /// <summary>
        /// Cria um período a partir de uma data.
        /// </summary>
        public static FinancialPeriod FromDate(DateTimeOffset date)
        {
            return new FinancialPeriod(date.Month, date.Year);
        }

        /// <summary>
        /// Retorna o período do mês anterior.
        /// </summary>
        public FinancialPeriod PreviousMonth()
        {
            if (Month == 1)
                return new FinancialPeriod(12, Year - 1);

            return new FinancialPeriod(Month - 1, Year);
        }

        /// <summary>
        /// Retorna o período do próximo mês.
        /// </summary>
        public FinancialPeriod NextMonth()
        {
            if (Month == 12)
                return new FinancialPeriod(1, Year + 1);

            return new FinancialPeriod(Month + 1, Year);
        }

        /// <summary>
        /// Retorna a data de início do período (primeiro dia do mês).
        /// </summary>
        public DateTimeOffset GetStartDate()
        {
            return new DateTimeOffset(Year, Month, 1, 0, 0, 0, TimeSpan.Zero);
        }

        /// <summary>
        /// Retorna a data de fim do período (último dia do mês).
        /// </summary>
        public DateTimeOffset GetEndDate()
        {
            var daysInMonth = DateTime.DaysInMonth(Year, Month);
            return new DateTimeOffset(Year, Month, daysInMonth, 23, 59, 59, TimeSpan.Zero);
        }

        /// <summary>
        /// Verifica se o período é o mês/ano atual.
        /// </summary>
        public bool IsCurrent()
        {
            var now = DateTimeOffset.UtcNow;
            return Month == now.Month && Year == now.Year;
        }

        public override string ToString() => $"{Month:D2}/{Year}";

        /// <summary>
        /// Formata o período como nome do mês e ano (ex: "Janeiro 2024").
        /// </summary>
        public string ToLongString()
        {
            var monthName = new DateTimeOffset(Year, Month, 1, 0, 0, 0, TimeSpan.Zero)
                .ToString("MMMM yyyy");
            return monthName;
        }
    }
}
