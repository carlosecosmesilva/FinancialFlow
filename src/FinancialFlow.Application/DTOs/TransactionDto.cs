using System;
using FinancialFlow.Domain.Enums;

namespace FinancialFlow.Application.DTOs;

public sealed record TransactionDto(
    Guid Id,
    string Description,
    decimal Amount,
    string Currency,
    DateTime Date,
    TransactionType Type
);