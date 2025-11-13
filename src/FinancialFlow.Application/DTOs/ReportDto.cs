namespace FinancialFlow.Application.DTOs;

public sealed record ReportDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public DateTime PeriodStart { get; init; }
    public DateTime PeriodEnd { get; init; }
    public string Currency { get; init; } = "BRL";
    public decimal Total { get; init; }
    public string? Url { get; init; }
    public DateTimeOffset GeneratedAt { get; init; }
}