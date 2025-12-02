namespace E_Commerce.Shared.DTOs.BasketDTOs;

public record BasketDTO (string Id  ,ICollection<BasketItemDTO> Items);