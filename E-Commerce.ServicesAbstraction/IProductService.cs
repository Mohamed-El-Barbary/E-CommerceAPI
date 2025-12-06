using E_Commerce.Shared;
using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.ProductDTOs;

namespace E_Commerce.Services_Abstraction;

public interface IProductService
{
    Task<PaginatedResult<ProductDTO>> GetAllProductsAsync(ProductQueryparams  queryparams);
    
    Task<Result<ProductDTO>> GetProductByIdAsync(int productId);
    
    Task<IEnumerable<BrandDTO>> GetAllBrandsAsync();

    Task<IEnumerable<TypeDTO>> GetAllTypesAsync();
}