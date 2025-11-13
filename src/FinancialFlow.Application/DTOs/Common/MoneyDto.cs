namespace FinancialFlow.Application.DTOs.Common;

public sealed record MoneyDto
{
    public decimal Amount { get; init; }
    public string Currency { get; init; } = string.Empty;
}
