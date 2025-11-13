using System;

namespace FinancialFlow.Application.DTOs;

public sealed record MonthlySummaryDto(
    Guid UserId,
    int Month,
    int Year,
    decimal TotalIncome,
    decimal TotalExpense,
    decimal Balance
);