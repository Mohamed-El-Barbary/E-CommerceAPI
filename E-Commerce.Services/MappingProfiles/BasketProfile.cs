using AutoMapper;
using E_Commerce.Domain.Entities.BasketModule;
using E_Commerce.Shared.DTOs.BasketDTOs;

namespace E_Commerce.Services.MappingProfiles;

public class BasketProfile : Profile
{

    public BasketProfile()
    {
        CreateMap<CustomerBasket, BasketDTO>().ReverseMap();
        CreateMap<BasketItem, BasketItemDTO>().ReverseMap();
    }
}