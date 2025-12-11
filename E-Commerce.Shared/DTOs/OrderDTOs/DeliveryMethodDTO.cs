namespace E_Commerce.Shared.DTOs.OrderDTOs
{
    public record DeliveryMethodDTO(
        int Id,
        string ShortName,
        string Description,
        string DeliveryTime,
        decimal Price
    );
}