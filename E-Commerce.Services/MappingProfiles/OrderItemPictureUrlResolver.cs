using AutoMapper;
using E_Commerce.Domain.Entities.OrderModule;
using E_Commerce.Shared.DTOs.OrderDTOs;
using Microsoft.Extensions.Configuration;

namespace E_Commerce.Services.MappingProfiles;

public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDTO, string>
{
    private readonly IConfiguration _configuration;

    public OrderItemPictureUrlResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Resolve(OrderItem source, OrderItemDTO destination, string destMember, ResolutionContext context)
    {

        if (string.IsNullOrEmpty(source.Product.PictureUrl))
            return string.Empty;

        if (source.Product.PictureUrl.StartsWith("http"))
            return source.Product.PictureUrl;

        var baseUrl = _configuration.GetSection("URLs")["BaseUrl"];

        if (string.IsNullOrEmpty(baseUrl))
            return string.Empty;

        var pictureUrl = $"{baseUrl}{source.Product.PictureUrl}";
        return pictureUrl;
    }
}