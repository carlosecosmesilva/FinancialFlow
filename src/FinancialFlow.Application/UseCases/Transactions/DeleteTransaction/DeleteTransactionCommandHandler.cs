using MediatR;
using Microsoft.Extensions.Logging;
using FinancialFlow.Domain.Interfaces;
using FinancialFlow.Application.Common.Results;

namespace FinancialFlow.Application.UseCases.Transactions.DeleteTransaction;

public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand, Result<Guid>>
{
    private readonly ITransactionRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteTransactionCommandHandler> _logger;

    public DeleteTransactionCommandHandler(
        ITransactionRepository repository,
        IUnitOfWork unitOfWork,
        ILogger<DeleteTransactionCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var transaction = await _repository.GetByIdAsync(request.TransactionId, cancellationToken);

            if (transaction == null)
            {
                _logger.LogWarning("Transaction {TransactionId} not found for deletion", request.TransactionId);
                return Result<Guid>.Failure("Transaction not found");
            }

            // Verify ownership
            if (transaction.UserId != request.UserId)
            {
                _logger.LogWarning(
                    "User {UserId} attempted to delete transaction {TransactionId} belonging to another user",
                    request.UserId,
                    request.TransactionId);
                return Result<Guid>.Failure("Unauthorized to delete this transaction");
            }

            await _repository.DeleteAsync(transaction, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            _logger.LogInformation(
                "Transaction {TransactionId} deleted successfully by user {UserId}",
                request.TransactionId,
                request.UserId);

            return Result<Guid>.Success(transaction.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting transaction {TransactionId}", request.TransactionId);
            return Result<Guid>.Failure("An unexpected error occurred while deleting the transaction");
        }
    }
}