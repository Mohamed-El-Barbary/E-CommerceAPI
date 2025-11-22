using System.Linq.Expressions;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Services.Specifications;

internal abstract class BaseSpecifications<TEntity , TKey> : ISpecifications<TEntity , TKey> where TEntity : BaseEntity<TKey>
{
    public ICollection<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];
    
    public Expression<Func<TEntity, bool>> Criteria { get; }

    protected BaseSpecifications(Expression<Func<TEntity, bool>> criteriaExpression )
    {
        Criteria = criteriaExpression;
    }

    protected void AddInclude(Expression<Func<TEntity, object>> includeExp)
    {
        IncludeExpressions.Add(includeExp);
    }
    
}