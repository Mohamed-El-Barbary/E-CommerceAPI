using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Shared.DTOs.BasketDTOs;

public record BasketItemDTO(
    int Id,
    string ProductName,
    string PictureUrl,
    [Range(1, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    decimal Price,
    [Range(1, 100)]
    int Quantity
    );