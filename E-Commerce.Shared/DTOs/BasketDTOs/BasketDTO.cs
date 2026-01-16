namespace E_Commerce.Shared.DTOs.BasketDTOs;

public record BasketDTO(string Id, decimal TotalPrice, int NumOfCartItems, ICollection<BasketItemDTO> Items);