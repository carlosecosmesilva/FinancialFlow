using MediatR;
using Microsoft.Extensions.Logging;
using FinancialFlow.Domain.Interfaces;
using FinancialFlow.Application.Common.Results;
using FinancialFlow.Application.DTOs;

namespace FinancialFlow.Application.UseCases.Transactions.GetTransactionById;

public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, Result<TransactionDto>>
{
    private readonly ITransactionRepository _repository;
    private readonly ILogger<GetTransactionByIdQueryHandler> _logger;

    public GetTransactionByIdQueryHandler(
        ITransactionRepository repository,
        ILogger<GetTransactionByIdQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<TransactionDto>> Handle(
        GetTransactionByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var transaction = await _repository.GetByIdAsync(request.TransactionId, cancellationToken);

            if (transaction == null)
            {
                _logger.LogWarning("Transaction {TransactionId} not found", request.TransactionId);
                return Result<TransactionDto>.Failure("Transaction not found");
            }

            var dto = new TransactionDto
            {
                Id = transaction.Id,
                UserId = transaction.UserId,
                Description = transaction.Description,
                Amount = transaction.Value.Amount,
                Currency = transaction.Value.Currency,
                Type = transaction.Type,
                Date = transaction.TransactionDate.DateTime,
                Category = transaction.Category,
                Notes = transaction.Notes,
                CreatedAt = transaction.CreatedAt
            };

            return Result<TransactionDto>.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving transaction {TransactionId}", request.TransactionId);
            return Result<TransactionDto>.Failure(
                "An unexpected error occurred while retrieving the transaction");
        }
    }
}