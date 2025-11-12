using MediatR;
using FinancialFlow.Domain.Interfaces;
using FinancialFlow.Domain.ValueObjects;
using FinancialFlow.Domain.Enums;
using FinancialFlow.Domain.Exceptions;
using FinancialFlow.Application.Common.Results;
using Microsoft.Extensions.Logging;

namespace FinancialFlow.Application.UseCases.Transactions.UpdateTransaction;

public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand, Result<Guid>>
{
    private readonly ITransactionRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateTransactionCommandHandler> _logger;

    public UpdateTransactionCommandHandler(
            ITransactionRepository repository,
            IUnitOfWork unitOfWork,
            ILogger<UpdateTransactionCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingTransaction = await _repository.GetByIdAsync(request.TransactionId, cancellationToken);
            if (existingTransaction == null)
            {
                _logger.LogWarning("Transaction {TransactionId} not found for update", request.TransactionId);
                return Result<Guid>.Failure("Transaction not found");
            }

            // Use domain methods to update entity
            if (!string.IsNullOrWhiteSpace(request.Description))
                existingTransaction.UpdateDescription(request.Description);

            var money = new Money(request.Amount, request.Currency);
            existingTransaction.UpdateValue(money);

            existingTransaction.UpdateTransactionDate(request.TransactionDate);

            if (request.Category != null)
                existingTransaction.UpdateCategory(request.Category);

            if (request.Notes != null)
                existingTransaction.UpdateNotes(request.Notes);

            // Repository Update method is typically void, just marks as modified
            _repository.Update(existingTransaction);

            // Commit unit of work
            await _unitOfWork.CommitAsync(cancellationToken);

            _logger.LogInformation(
                "Transaction {TransactionId} updated successfully for user {UserId}",
                existingTransaction.Id,
                existingTransaction.UserId);

            return Result<Guid>.Success(existingTransaction.Id);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain validation failed while updating transaction");
            return Result<Guid>.Failure(ex.Message);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument while updating transaction");
            return Result<Guid>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error updating transaction");
            return Result<Guid>.Failure("An unexpected error occurred while updating the transaction");
        }
    }
}