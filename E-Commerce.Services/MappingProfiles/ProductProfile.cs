using AutoMapper;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Shared.DTOs.ProductDTOs;

namespace E_Commerce.Services.MappingProfiles;

public class ProductProfile : Profile
{

    public ProductProfile()
    {
        CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.ProductBrand, opt => opt.MapFrom(src => src.ProductBrand.Name))
            .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType.Name))
            .ForMember(dest => dest.ProductSubType, opt => opt.MapFrom(src => src.ProductSubType.Name))
            .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<ProductPictureUrlResolver>())
            .ForMember(dest => dest.ProductSizes, opt => opt.MapFrom(src => src.ProductSizes.Select(s => s.Size).ToList()))
            .ForMember(dest => dest.ProductColors, opt => opt.MapFrom(src => src.ProductColors.Select(c => new ColorDTO
            {
                Color = c.ColorName,
                HexValue = c.HexValue
            }).ToList()))
            .ForMember(dest => dest.Images, opt => opt.MapFrom<ProductImagesUrlResolver>()); 

        CreateMap<ProductBrand, BrandDTO>();
        CreateMap<ProductType, TypeDTO>()
        .ForMember(dest => dest.SubTypes,
            opt => opt.MapFrom(src => src.ProductSubTypes
                                        .Select(s => new SubTypesDTO
                                        {
                                            Id = s.Id,
                                            Name = s.Name
                                        }).ToList()));
        CreateMap<ProductSubType, SubTypesDTO>();

    }

}