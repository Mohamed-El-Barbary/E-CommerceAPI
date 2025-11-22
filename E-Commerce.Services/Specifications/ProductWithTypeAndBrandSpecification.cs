using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Shared;

namespace E_Commerce.Services.Specifications;

internal class ProductWithTypeAndBrandSpecification : BaseSpecifications<Product, int>
{

    public ProductWithTypeAndBrandSpecification(int id) : base(p => p.Id == id)
    {
        AddInclude(p => p.ProductBrand);
        AddInclude(p => p.ProductType);
    }
    
    public ProductWithTypeAndBrandSpecification(ProductQueryparams  queryparams) : 
        base(p=> (!queryparams.BrandId.HasValue || p.BrandId == queryparams.BrandId.Value)
        && (!queryparams.TypeId.HasValue || p.TypeId == queryparams.TypeId.Value)
        && (string.IsNullOrEmpty(queryparams.Search) || p.Name.ToLower().Contains(queryparams.Search.ToLower()) ))
    {
        AddInclude(p => p.ProductBrand);
        AddInclude(p => p.ProductType);
    }
    
}