using AutoMapper;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Shared.DTOs.ProductDTOs;
using Microsoft.Extensions.Configuration;

namespace E_Commerce.Services.MappingProfiles;

public class ProductImagesUrlResolver : IValueResolver<Product, ProductDTO, ICollection<string>?>
{
    private readonly IConfiguration _configuration;

    public ProductImagesUrlResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ICollection<string>? Resolve(Product source, ProductDTO destination, ICollection<string>? destMember, ResolutionContext context)
    {
        if (source.ProductImages == null || !source.ProductImages.Any())
            return new List<string> { "Not Found Images" };

        var baseUrl = _configuration.GetSection("URLs")["BaseUrl"] ?? string.Empty;

        return source.ProductImages
                     .Select(img => string.IsNullOrEmpty(img.PictureUrl) ? "Not Found Images"
                              : img.PictureUrl.StartsWith("http") ? img.PictureUrl
                              : $"{baseUrl}{img.PictureUrl}")
                     .ToList();
    }
}
