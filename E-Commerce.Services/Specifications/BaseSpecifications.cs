using System.Linq.Expressions;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Services.Specifications;

internal abstract class BaseSpecifications<TEntity , TKey> : ISpecifications<TEntity , TKey> where TEntity : BaseEntity<TKey>
{
    public ICollection<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];

    protected void AddInclude(Expression<Func<TEntity, object>> includeExp)
    {
        IncludeExpressions.Add(includeExp);
    }
    
}