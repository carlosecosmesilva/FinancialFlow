using MediatR;
using FinancialFlow.Application.Common.Results;
using FinancialFlow.Application.DTOs;

namespace FinancialFlow.Application.UseCases.Transactions.GetTransactionById;

public sealed record GetTransactionByIdQuery(Guid TransactionId) : IRequest<Result<TransactionDto>>;