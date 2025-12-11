using E_Commerce.Presentation.Attributes;
using E_Commerce.Services_Abstraction;
using E_Commerce.Shared;
using E_Commerce.Shared.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Presentation.Controllers;


public class ProductsController : ApiBaseController
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    //[Authorize]
    [HttpGet ]
    [RedisCache]
    public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProducts([FromQuery] ProductQueryparams queryparams)
    {
        var products = await _productService.GetAllProductsAsync(queryparams);
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDTO>> GetProduct(int id)
    {
        var result = await _productService.GetProductByIdAsync(id);
        return HandleResult<ProductDTO>(result);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IEnumerable<BrandDTO>>> GetAllBrands()
    {
        var brands = await _productService.GetAllBrandsAsync();
        return Ok(brands);
    }

    [HttpGet("types")]
    public async Task<ActionResult<IEnumerable<TypeDTO>>> GetAllTypes()
    {
        var types = await _productService.GetAllTypesAsync();
        return Ok(types);
    }
}