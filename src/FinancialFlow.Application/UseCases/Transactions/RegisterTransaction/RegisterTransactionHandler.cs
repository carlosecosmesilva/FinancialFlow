using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using FinancialFlow.Application.Common.Results;
using FinancialFlow.Domain.Entities;
using FinancialFlow.Domain.Enums;
using FinancialFlow.Domain.Interfaces;
using FinancialFlow.Domain.ValueObjects;

namespace FinancialFlow.Application.UseCases.Transactions.Commands.RegisterTransaction;

public sealed class RegisterTransactionHandler
    : IRequestHandler<RegisterTransactionCommand, Result<Guid>>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterTransactionHandler(
        ITransactionRepository transactionRepository,
        IUnitOfWork unitOfWork)
    {
        _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result<Guid>> Handle(RegisterTransactionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Adapta ao padrão atual do domínio (Money + factory da entidade)
            var money = new Money(request.Amount, request.Currency);

            var transaction = FinancialTransaction.Create(
                request.UserId,
                money,
                request.Type,
                request.Description,
                request.TransactionDate
            );

            await _transactionRepository.AddAsync(transaction, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Result<Guid>.Success(transaction.Id);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure(ex.Message);
        }
    }
}