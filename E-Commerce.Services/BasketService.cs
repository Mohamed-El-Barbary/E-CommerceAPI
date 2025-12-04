using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.BasketModule;
using E_Commerce.Services_Abstraction;
using E_Commerce.Services.Exceptions;
using E_Commerce.Shared.DTOs.BasketDTOs;

namespace E_Commerce.Services;

public class BasketService : IBasketService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IMapper _mapper;

    public BasketService(IBasketRepository basketRepository, IMapper mapper)
    {
        _basketRepository = basketRepository;
        _mapper = mapper;
    }

    public async Task<BasketDTO> GetBasketAsync(string basketId)
    {
        var basket = await _basketRepository.GetBasketAsync(basketId);
        if (basket is null)
            throw new BasketNotFoundExceptions(basketId);
        return _mapper.Map<CustomerBasket,BasketDTO>(basket!);
    }

    public async Task<BasketDTO> CreateOrUpdateBasketAsync(BasketDTO basket)
    {
        var customerBasket = _mapper.Map<BasketDTO, CustomerBasket>(basket);
        var createOrUpdateBasket = await _basketRepository.CreateOrUpdateBasketAsync(customerBasket);
        return _mapper.Map<CustomerBasket, BasketDTO>(createOrUpdateBasket!);
    }

    public async Task<bool> DeleteBasketAsync(string basketId) => await _basketRepository.DeleteBasketAsync(basketId);
}