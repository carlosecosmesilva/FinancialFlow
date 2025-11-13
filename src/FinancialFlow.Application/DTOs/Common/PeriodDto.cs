using System;

namespace FinancialFlow.Application.DTOs.Common;

public sealed record PeriodDto
{
    public int Month { get; init; }
    public int Year { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }

    public PeriodDto() { }

    public PeriodDto(int month, int year)
    {
        Month = month;
        Year = year;
        StartDate = new DateTime(year, month, 1);
        EndDate = StartDate.AddMonths(1).AddDays(-1);
    }
}