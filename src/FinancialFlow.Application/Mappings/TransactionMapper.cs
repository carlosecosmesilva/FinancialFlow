using System.Collections.Generic;
using FinancialFlow.Domain.Entities;
using FinancialFlow.Application.DTOs;
using FinancialFlow.Application.DTOs.Common;
using FinancialFlow.Application.UseCases.Transactions.GetTransactions;

namespace FinancialFlow.Application.Mappings;

public static class TransactionMapper
{
    public static TransactionDto ToDto(this FinancialTransaction t)
        => new TransactionDto
        {
            Id = t.Id,
            UserId = t.UserId,
            Description = t.Description,
            Amount = t.Value.Amount,
            Currency = t.Value.Currency,
            Type = t.Type,
            Date = t.TransactionDate.DateTime,
            Category = t.Category,
            Notes = t.Notes,
            CreatedAt = t.CreatedAt
        };

    public static TransactionListDto ToListDto(this FinancialTransaction t)
        => new TransactionListDto
        {
            Id = t.Id,
            Description = t.Description,
            Amount = t.Value.Amount,
            Currency = t.Value.Currency,
            Type = t.Type.ToString(),
            TransactionDate = t.TransactionDate.DateTime,
            Category = t.Category,
            Notes = t.Notes
        };

    public static PaginatedListDto<TOut> ToPaginatedDto<TSource, TOut>(
        IEnumerable<TSource> items,
        int totalCount,
        int pageNumber,
        int pageSize,
        System.Func<TSource, TOut> projector)
    {
        var list = new List<TOut>();
        foreach (var i in items)
            list.Add(projector(i));

        return new PaginatedListDto<TOut>(list, totalCount, pageNumber, pageSize);
    }
}
