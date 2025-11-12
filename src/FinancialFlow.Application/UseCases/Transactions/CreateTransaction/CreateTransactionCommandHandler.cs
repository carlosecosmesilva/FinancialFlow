using MediatR;
using Microsoft.Extensions.Logging;
using FinancialFlow.Domain.Interfaces;
using FinancialFlow.Domain.ValueObjects;
using FinancialFlow.Domain.Enums;
using FinancialFlow.Domain.Exceptions;
using FinancialFlow.Domain.Entities;
using FinancialFlow.Application.Common.Results;

namespace FinancialFlow.Application.UseCases.Transactions.CreateTransaction;

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Result<Guid>>
{
    private readonly ITransactionRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateTransactionCommandHandler> _logger;

    public CreateTransactionCommandHandler(
        ITransactionRepository repository,
        IUnitOfWork unitOfWork,
        ILogger<CreateTransactionCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Parse transaction type
            if (!Enum.TryParse<TransactionType>(request.Type, ignoreCase: true, out var type))
            {
                return Result<Guid>.Failure("Invalid transaction type");
            }

            // Create Money value object
            var money = new Money(request.Amount, request.Currency);

            // Use factory method from domain entity
            var transaction = FinancialTransaction.Create(
                userId: request.UserId,
                value: money,
                type: type,
                description: request.Description,
                transactionDate: request.TransactionDate,
                category: request.Category,
                notes: request.Notes);

            // Add to repository
            await _repository.AddAsync(transaction, cancellationToken);

            // Commit unit of work
            await _unitOfWork.CommitAsync(cancellationToken);

            _logger.LogInformation(
                "Transaction {TransactionId} created successfully for user {UserId}",
                transaction.Id,
                request.UserId);

            return Result<Guid>.Success(transaction.Id);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain validation failed while creating transaction");
            return Result<Guid>.Failure(ex.Message);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument while creating transaction");
            return Result<Guid>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error creating transaction");
            return Result<Guid>.Failure("An unexpected error occurred while creating the transaction");
        }
    }
}
