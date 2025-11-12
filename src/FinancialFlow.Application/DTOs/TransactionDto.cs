using System;
using FinancialFlow.Domain.Enums;

namespace FinancialFlow.Application.DTOs;

public sealed record TransactionDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string Description { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public string Currency { get; init; } = string.Empty;
    public TransactionType Type { get; init; }
    public DateTime Date { get; init; }
    public string? Category { get; init; }
    public string? Notes { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
}