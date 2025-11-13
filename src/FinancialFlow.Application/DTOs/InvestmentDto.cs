using FinancialFlow.Domain.Enums;

namespace FinancialFlow.Application.DTOs;

public sealed record InvestmentDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string Name { get; init; } = string.Empty;
    public InvestmentType Type { get; init; }
    public decimal InitialAmount { get; init; }
    public decimal CurrentValue { get; init; }
    public string Currency { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
}