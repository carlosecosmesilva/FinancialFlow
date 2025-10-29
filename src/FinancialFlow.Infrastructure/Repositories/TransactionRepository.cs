using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinancialFlow.Domain.Entities;
using FinancialFlow.Domain.Enums;
using FinancialFlow.Domain.Interfaces;
using FinancialFlow.Infrastructure.Data;

namespace FinancialFlow.Infrastructure.Repositories;

public sealed class TransactionRepository(ApplicationDbContext context) : ITransactionRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task AddAsync(FinancialTransaction transaction, CancellationToken cancellationToken = default)
    {
        await _context.FinancialTransactions.AddAsync(transaction, cancellationToken);
    }

    public async Task<FinancialTransaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.FinancialTransactions
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<FinancialTransaction>> GetByPeriodAsync(
        Guid userId,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        CancellationToken cancellationToken = default)
    {
        return await _context.FinancialTransactions
            .Where(t => EF.Property<Guid>(t, "UserId") == userId
                && t.TransactionDate >= startDate
                && t.TransactionDate <= endDate)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetCategoryTotalAsync(
        Guid userId,
        string category,
        int month,
        int year,
        CancellationToken cancellationToken = default)
    {
        var startDate = new DateTimeOffset(year, month, 1, 0, 0, 0, TimeSpan.Zero);
        var endDate = startDate.AddMonths(1).AddSeconds(-1);

        return await _context.FinancialTransactions
            .Where(t => EF.Property<Guid>(t, "UserId") == userId
                && t.Description.Contains(category)
                && t.TransactionDate >= startDate
                && t.TransactionDate <= endDate)
            .SumAsync(t => t.Value.Amount, cancellationToken);
    }

    public async Task<IEnumerable<FinancialTransaction>> GetByTypeAsync(
        Guid userId,
        TransactionType type,
        CancellationToken cancellationToken = default)
    {
        return await _context.FinancialTransactions
            .Where(t => EF.Property<Guid>(t, "UserId") == userId && t.Type == type)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<FinancialTransaction>> GetByUserAsync(
        Guid userId,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        return await _context.FinancialTransactions
            .Where(t => EF.Property<Guid>(t, "UserId") == userId)
            .OrderByDescending(t => t.TransactionDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetTotalRevenueAsync(
        Guid userId,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        CancellationToken cancellationToken = default)
    {
        return await _context.FinancialTransactions
            .Where(t => EF.Property<Guid>(t, "UserId") == userId
                && t.Type == TransactionType.Revenue
                && t.TransactionDate >= startDate
                && t.TransactionDate <= endDate)
            .SumAsync(t => t.Value.Amount, cancellationToken);
    }

    public async Task<decimal> GetTotalExpenseAsync(
        Guid userId,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        CancellationToken cancellationToken = default)
    {
        return await _context.FinancialTransactions
            .Where(t => EF.Property<Guid>(t, "UserId") == userId
                && t.Type == TransactionType.Expense
                && t.TransactionDate >= startDate
                && t.TransactionDate <= endDate)
            .SumAsync(t => t.Value.Amount, cancellationToken);
    }

    public void Update(FinancialTransaction transaction)
    {
        _context.FinancialTransactions.Update(transaction);
    }
}