using System;
using MediatR;
using FinancialFlow.Application.Common.Results;
using FinancialFlow.Domain.Enums;

namespace FinancialFlow.Application.UseCases.Transactions.Commands.RegisterTransaction;

public sealed record RegisterTransactionCommand(
    Guid UserId,
    string Description,
    decimal Amount,
    string Currency,
    TransactionType Type,
    DateTime TransactionDate
) : IRequest<Result<Guid>>;