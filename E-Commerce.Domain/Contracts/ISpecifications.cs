using System.Linq.Expressions;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Domain.Contracts;

public interface ISpecifications<TEntity , TKey> where TEntity : BaseEntity<TKey>
{
    
    public ICollection<Expression<Func<TEntity, object>>> IncludeExpressions { get; }
    
}