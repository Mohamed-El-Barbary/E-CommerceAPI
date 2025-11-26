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

    public ProductWithTypeAndBrandSpecification(ProductQueryparams queryParams) :
        base(p => (!queryParams.BrandId.HasValue || p.BrandId == queryParams.BrandId.Value)
                  && (!queryParams.TypeId.HasValue || p.TypeId == queryParams.TypeId.Value)
                  && (string.IsNullOrEmpty(queryParams.Search) ||
                      p.Name.ToLower().Contains(queryParams.Search.ToLower())))
    {
        AddInclude(p => p.ProductBrand);
        AddInclude(p => p.ProductType);

        switch (queryParams.Sort)
        {
            case ProductSortingOptions.NameAsc:
                AddOrderBy(orderByExp: x => x.Name);
                break;
            case ProductSortingOptions.NameDesc:
                AddOrderByDescending(orderByDescendingExp: x => x.Name);
                break;
            case ProductSortingOptions.PriceAsc:
                AddOrderBy(orderByExp: x => x.Price);
                break;
            case ProductSortingOptions.PriceDesc:
                AddOrderByDescending(orderByDescendingExp: x => x.Price);
                break;
            
            default:
                AddOrderBy(orderByExp: x => x.Id);
                break;
        }
    }
}