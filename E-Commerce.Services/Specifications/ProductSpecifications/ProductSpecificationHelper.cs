using System.Linq.Expressions;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Shared;

namespace E_Commerce.Services.Specifications.ProductSpecifications;

internal static class ProductSpecificationHelper
{
    public static Expression<Func<Product, bool>> GetProductCriteria(ProductQueryparams queryParams)
    {
        return p => (!queryParams.BrandId.HasValue || p.BrandId == queryParams.BrandId.Value)
                    && (!queryParams.TypeId.HasValue || p.TypeId == queryParams.TypeId.Value)
                    && (!queryParams.SubTypeId.HasValue || p.ProductSubTypeId == queryParams.SubTypeId.Value)
                    && (string.IsNullOrEmpty(queryParams.Search) || p.Name.ToLower().Contains(queryParams.Search.ToLower()));
    }
}