using Microsoft.EntityFrameworkCore;
using FinancialFlow.Domain.Entities;
using FinancialFlow.Domain.Interfaces;
using FinancialFlow.Domain.ValueObjects;
using FinancialFlow.Infrastructure.Data;

namespace FinancialFlow.Infrastructure.Repositories;

public sealed class UserRepository(ApplicationDbContext context) : IUserRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Set<User>().AddAsync(user, cancellationToken);
    }

    public Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return _context.Set<User>()
            .FirstOrDefaultAsync(u => u.Email.Address == email.Address, cancellationToken);
    }

    public Task<bool> EmailExistsAsync(Email email, CancellationToken cancellationToken = default)
    {
        return _context.Set<User>()
            .AnyAsync(u => u.Email.Address == email.Address, cancellationToken);
    }

    public void Update(User entity)
    {
        _context.Set<User>().Update(entity);
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Set<User>()
            .FindAsync(new object[] { id }, cancellationToken)
            .AsTask();
    }
}