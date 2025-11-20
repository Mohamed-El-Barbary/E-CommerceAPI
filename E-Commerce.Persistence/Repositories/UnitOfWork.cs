using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using E_Commerce.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace E_Commerce.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly StoreDbContext _dbContext;
    private readonly Dictionary<Type, object> _repositories = [];

    public UnitOfWork(StoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();

    public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
    {
        var entityType = typeof(TEntity);

        if (_repositories.TryGetValue(entityType, out object? repository))
            return (IGenericRepository<TEntity, TKey>)repository;

        var newRepo = new GenericRepository<TEntity, TKey>(_dbContext);

        _repositories[entityType] = newRepo;
        return newRepo;
    }
}