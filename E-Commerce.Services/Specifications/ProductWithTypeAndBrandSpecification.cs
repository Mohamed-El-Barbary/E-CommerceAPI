using E_Commerce.Domain.Entities.ProductModule;

namespace E_Commerce.Services.Specifications;

internal class ProductWithTypeAndBrandSpecification : BaseSpecifications<Product, int>
{

    public ProductWithTypeAndBrandSpecification(int id) : base(p => p.Id == id)
    {
        AddInclude(p => p.ProductBrand);
        AddInclude(p => p.ProductType);
    }
    
    public ProductWithTypeAndBrandSpecification() : base(null)
    {
        AddInclude(p => p.ProductBrand);
        AddInclude(p => p.ProductType);
    }
    
}