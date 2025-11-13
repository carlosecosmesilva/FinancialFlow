namespace FinancialFlow.Application.DTOs;

public sealed record DebtDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string Creditor { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public string Currency { get; init; } = string.Empty;
    public DateTime DueDate { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
}