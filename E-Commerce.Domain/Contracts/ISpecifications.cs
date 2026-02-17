using System.Linq.Expressions;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Domain.Contracts;

public interface ISpecifications<TEntity , TKey> where TEntity : BaseEntity<TKey>
{
    
    public ICollection<Expression<Func<TEntity, object>>> IncludeExpressions { get; }
    public Expression<Func<TEntity,bool>> Criteria { get; }
    public Expression<Func<TEntity, object>> OrderBy { get; }
    public Expression<Func<TEntity, object>> OrderByDescending { get; }
    public int Take { get; }
    public int Skip { get; }
    public bool IsPaginated { get; }
    
}