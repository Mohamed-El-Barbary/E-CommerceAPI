using System.Linq.Expressions;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Services.Specifications;

internal abstract class BaseSpecifications<TEntity, TKey> : ISpecifications<TEntity, TKey>
    where TEntity : BaseEntity<TKey>
{
    public Expression<Func<TEntity, bool>> Criteria { get; }

    protected BaseSpecifications(Expression<Func<TEntity, bool>> criteriaExpression)
    {
        Criteria = criteriaExpression;
    }

    #region Include

    public ICollection<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];

    protected void AddInclude(Expression<Func<TEntity, object>> includeExp)
    {
        IncludeExpressions.Add(includeExp);
    }

    #endregion

    #region Sorting

    public Expression<Func<TEntity, object>> OrderBy { get; private set; }
    public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }

    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExp)
    {
        OrderBy =  orderByExp;
    } 
    protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExp)
    {
        OrderByDescending =  orderByDescendingExp;
    }

    #endregion

    #region Pagination
    
    public int Take { get; private set; }
    public int Skip { get; private set; }
    public bool IsPaginated { get; private set; }

    protected void ApplyPagination(int pageSize, int pageIndex)
    {

        IsPaginated = true;
        Take = pageSize;
        Skip = (pageIndex - 1) * pageSize;

    }

    #endregion
    
}