using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Services.Exceptions;
using E_Commerce.Services.Specifications;
using E_Commerce.Services_Abstraction;
using E_Commerce.Shared;
using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.ProductDTOs;
using System;

namespace E_Commerce.Services;

public class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
{
    public async Task<PaginatedResult<ProductDTO>> GetAllProductsAsync(ProductQueryparams queryparams)
    {
        var spec = new ProductWithTypeAndBrandSpecification(queryparams);
        var products = await unitOfWork.GetRepository<Product, int>().GetAllAsync(spec);
        var dataToReturn = mapper.Map<IEnumerable<ProductDTO>>(products);
        var countOfReturnedData = dataToReturn.Count();
        var countSpec = new ProductCountSpecification(queryparams);
        var countOfAllProducts = await unitOfWork.GetRepository<Product, int>().CountAsync(countSpec);
        return new PaginatedResult<ProductDTO>(dataToReturn,countOfAllProducts,countOfReturnedData, queryparams.PageIndex);
    }

    public async Task<Result<ProductDTO>> GetProductByIdAsync(int productId)
    {
        var spec = new ProductWithTypeAndBrandSpecification(productId);
        var product = await unitOfWork.GetRepository<Product, int>().GetByIdAsync(spec);
        if (product is null)
            return Error.NotFound("Product not found", $"Product With Id {productId} not found");
        return mapper.Map<ProductDTO>(product);
    }

    public async Task<IEnumerable<BrandDTO>> GetAllBrandsAsync()
    {
        var brands = await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
        return mapper.Map<IEnumerable<BrandDTO>>(brands);
    }

    public async Task<IEnumerable<TypeDTO>> GetAllTypesAsync()
    {
        var typeSpec = new TypesWithSubTypesSpecification();
        var types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync(typeSpec);
        return mapper.Map<IEnumerable<TypeDTO>>(types);
    }

    public async Task<IEnumerable<TypeDTO>> GetAllTypesWithSubTypes(int typeId)
    {
        var typeSpec = new TypesWithSubTypesSpecification(typeId);
        var types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync(typeSpec);
        return mapper.Map<IEnumerable<TypeDTO>>(types);
    }
}