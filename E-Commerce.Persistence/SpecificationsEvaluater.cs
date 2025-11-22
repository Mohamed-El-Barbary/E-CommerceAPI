using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace E_Commerce.Persistence;

internal static class SpecificationsEvaluater
{
    public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> entryPoint,
        ISpecifications<TEntity, TKey> specifications) where TEntity : BaseEntity<TKey>
    {
        var query = entryPoint;

        if (specifications is not null)
        {

            if (specifications.Criteria is not null)
            {
                query = query.Where(specifications.Criteria);
            }
            
            if (specifications.IncludeExpressions is not null && specifications.IncludeExpressions.Any())
            {
                query = specifications.IncludeExpressions.Aggregate(
                    query,
                    (currentQuery, IncludeExp)
                        => currentQuery.Include(IncludeExp));
            }
        }

        return query;
    }
}