using System;
using MediatR;

namespace FinancialFlow.Application.UseCases.Reports.Queries.GetMonthlySummary;

public sealed record GetMonthlySummaryQuery(
    Guid UserId,
    int Month,
    int Year
) : IRequest<MonthlySummaryDto>;