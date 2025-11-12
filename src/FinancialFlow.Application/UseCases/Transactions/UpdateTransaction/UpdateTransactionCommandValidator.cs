using FluentValidation;
using FinancialFlow.Domain.Enums;

namespace FinancialFlow.Application.UseCases.Transactions.UpdateTransaction;

public class UpdateTransactionCommandValidator : AbstractValidator<UpdateTransactionCommand>
{
    public UpdateTransactionCommandValidator()
    {
        RuleFor(x => x.TransactionId)
            .NotEmpty().WithMessage("Transaction ID is required");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required")
            .Length(3).WithMessage("Currency must be 3 characters (e.g., BRL, USD)");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Transaction type is required")
            .Must(BeAValidType).WithMessage("Invalid transaction type");

        RuleFor(x => x.TransactionDate)
            .NotEmpty().WithMessage("Transaction date is required")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Transaction date cannot be in the future");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Category is required");
    }

    private bool BeAValidType(string type)
    {
        return Enum.TryParse<TransactionType>(type, ignoreCase: true, out _);
    }
}