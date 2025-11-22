using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Services_Abstraction;
using E_Commerce.Services.Specifications;
using E_Commerce.Shared.DTOs.ProductDTOs;

namespace E_Commerce.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync(int? brandId, int? typesId)
    {
        var spec = new ProductWithTypeAndBrandSpecification(brandId,typesId);
        var products = await _unitOfWork.GetRepository<Product, int>().GetAllAsync(spec);
        return _mapper.Map<IEnumerable<ProductDTO>>(products);
    }

    public async Task<ProductDTO> GetProductByIdAsync(int productId)
    {
        var spec = new ProductWithTypeAndBrandSpecification(productId);
        var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(spec);
        return _mapper.Map<ProductDTO>(product);
    }

    public async Task<IEnumerable<BrandDTO>> GetAllBrandsAsync()
    {
        var brands = await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
        return _mapper.Map<IEnumerable<BrandDTO>>(brands);
    }

    public async Task<IEnumerable<TypeDTO>> GetAllTypesAsync()
    {
        var types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
        return _mapper.Map<IEnumerable<TypeDTO>>(types);
    }
}