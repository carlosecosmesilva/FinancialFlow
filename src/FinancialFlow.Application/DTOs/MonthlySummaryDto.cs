using System;

namespace FinancialFlow.Application.UseCases.Reports.Queries.GetMonthlySummary;

public sealed record MonthlySummaryDto(
    Guid UserId,
    int Month,
    int Year,
    decimal TotalIncome,
    decimal TotalExpense,
    decimal Balance
);