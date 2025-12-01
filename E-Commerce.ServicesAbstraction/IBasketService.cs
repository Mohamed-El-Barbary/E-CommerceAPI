using E_Commerce.Shared.DTOs.BasketDTOs;

namespace E_Commerce.Services_Abstraction;

public interface IBasketService
{
    
    Task<BasketDTO> GetBasketAsync(string basketId);
    
    Task<BasketDTO> CreateOrUpdateBasketAsync(BasketDTO basket);
    
    Task<bool> DeleteBasketAsync(string basketId);
    
}