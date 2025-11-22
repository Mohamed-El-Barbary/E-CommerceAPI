using E_Commerce.Domain.Entities.ProductModule;

namespace E_Commerce.Services.Specifications;

internal class ProductWithTypeAndBrandSpecification : BaseSpecifications<Product, int>
{

    public ProductWithTypeAndBrandSpecification() : base()
    {
        AddInclude(p => p.ProductBrand);
        AddInclude(p => p.ProductType);
    }
    
}