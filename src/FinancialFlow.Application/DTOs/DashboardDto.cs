using System.Collections.Generic;
using FinancialFlow.Application.DTOs.Common;
using FinancialFlow.Application.UseCases.Transactions.GetTransactions;

namespace FinancialFlow.Application.DTOs;

public sealed record DashboardDto
{
    public Guid UserId { get; init; }
    public decimal TotalRevenue { get; init; }
    public decimal TotalExpense { get; init; }
    public decimal Balance { get; init; }
    public IEnumerable<MonthlySummaryDto>? MonthlySummaries { get; init; }
    public IEnumerable<TransactionListDto>? RecentTransactions { get; init; }
}