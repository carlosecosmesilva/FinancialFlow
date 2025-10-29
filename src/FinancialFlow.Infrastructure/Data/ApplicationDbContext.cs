using System.Reflection;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FinancialFlow.Domain.Entities;
using FinancialFlow.Domain.Interfaces;

namespace FinancialFlow.Infrastructure.Data;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<FinancialTransaction> FinancialTransactions => Set<FinancialTransaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await DispatchDomainEventsAsync(cancellationToken);
        return await base.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        => await SaveChangesAsync(cancellationToken);

    private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
    {
        // Descobre entidades que expõem DomainEvents (IEnumerable<INotification>) e limpa após publicar
        var domainEventEntities = ChangeTracker
            .Entries()
            .Select(e => e.Entity)
            .Where(e => e is not null)
            .ToList();

        var notifications = new List<INotification>();

        foreach (var entity in domainEventEntities)
        {
            var domainEventsProp = entity.GetType().GetProperty("DomainEvents", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (domainEventsProp?.GetValue(entity) is IEnumerable<INotification> domainEvents && domainEvents.Any())
            {
                notifications.AddRange(domainEvents);

                var clearMethod = entity.GetType().GetMethod("ClearDomainEvents", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                clearMethod?.Invoke(entity, null);
            }
        }

        foreach (var domainEvent in notifications)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }
    }
}