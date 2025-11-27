using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Shared;

namespace E_Commerce.Services.Specifications;

internal class ProductCountSpecification : BaseSpecifications<Product, int>
{
    public ProductCountSpecification(ProductQueryparams queryParams) :
        base(ProductSpecificationHelper.GetProductCriteria(queryParams))
    {
        
    }
}