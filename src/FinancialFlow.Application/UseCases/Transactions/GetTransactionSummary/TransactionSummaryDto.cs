namespace FinancialFlow.Application.UseCases.Transactions.GetTransactionSummary;

public sealed record TransactionSummaryDto
{
    public decimal TotalRevenue { get; init; }
    public decimal TotalExpense { get; init; }
    public decimal Balance { get; init; }
    public int TransactionCount { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public string Currency { get; init; } = "BRL";
}