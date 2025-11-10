using E_Commerce.Shared.DTOs.ProductDTOs;

namespace E_Commerce.Services_Abstraction;

public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
    
    Task<ProductDTO> GetProductByIdAsync(int productId);
    
    Task<IEnumerable<BrandDTO>> GetAllBrandsAsync();

    Task<IEnumerable<TypeDTO>> GetAllTypesAsync();
}