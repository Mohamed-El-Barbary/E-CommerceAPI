using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Shared;

namespace E_Commerce.Services.Specifications;

internal class ProductWithTypeAndBrandSpecification : BaseSpecifications<Product, int>
{
    public ProductWithTypeAndBrandSpecification(int id) : base(p => p.Id == id)
    {
        AddInclude(p => p.ProductBrand);
        AddInclude(p => p.ProductType);
        AddInclude(p => p.ProductSubType);
        AddInclude(p => p.ProductColors);
        AddInclude(p => p.ProductSizes);
        AddInclude(p => p.ProductImages);
    }

    public ProductWithTypeAndBrandSpecification(ProductQueryparams queryParams) :
        base(ProductSpecificationHelper.GetProductCriteria(queryParams))
    {
        AddInclude(p => p.ProductBrand);
        AddInclude(p => p.ProductType);
        AddInclude(p => p.ProductSubType);
        AddInclude(p => p.ProductColors);
        AddInclude(p => p.ProductSizes);
        AddInclude(p => p.ProductImages);


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
        
        ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        
    }           
}