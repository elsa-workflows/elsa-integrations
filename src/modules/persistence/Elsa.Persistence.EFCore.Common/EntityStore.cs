using Elsa.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Elsa.Persistence.EFCore;

/// <summary>
/// A generic repository class around EF Core for accessing entities that inherit from <see cref="Entity"/>.
/// </summary>
/// <typeparam name="TDbContext">The type of the database context.</typeparam>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public class EntityStore<TDbContext, TEntity>(IDbContextFactory<TDbContext> dbContextFactory, IServiceProvider serviceProvider) 
    : Store<TDbContext, TEntity>(dbContextFactory, serviceProvider) where TDbContext : DbContext where TEntity : Entity, new()
{
    /// <summary>
    /// Saves the entity.
    /// </summary>
    /// <param name="entity">The entity to save.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task SaveAsync(TEntity entity, CancellationToken cancellationToken = default) => await SaveAsync(entity, null, cancellationToken);
    
    /// <summary>
    /// Saves the entity.
    /// </summary>
    /// <param name="entity">The entity to save.</param>
    /// <param name="onSaving">The callback to invoke before saving the entity.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task SaveAsync(TEntity entity, Func<TDbContext, TEntity, CancellationToken, ValueTask>? onSaving, CancellationToken cancellationToken = default) => await SaveAsync(entity, x => x.Id, onSaving, cancellationToken);
    
    /// <summary>
    /// Saves the specified entities.
    /// </summary>
    /// <param name="entities">The entities to save.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task SaveManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) => await SaveManyAsync(entities, null, cancellationToken);
    
    /// <summary>
    /// Saves the specified entities.
    /// </summary>
    /// <param name="entities">The entities to save.</param>
    /// <param name="onSaving">The callback to invoke before saving the entities.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task SaveManyAsync(IEnumerable<TEntity> entities, Func<TDbContext, TEntity, CancellationToken, ValueTask>? onSaving = null, CancellationToken cancellationToken = default) => await SaveManyAsync(entities, x => x.Id, onSaving, cancellationToken);
}