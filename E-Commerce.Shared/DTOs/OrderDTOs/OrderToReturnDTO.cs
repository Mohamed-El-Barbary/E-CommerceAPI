namespace E_Commerce.Shared.DTOs.OrderDTOs
{
    public record OrderToReturnDTO(
        Guid Id,
        string UserEmail,
        ICollection<OrderItemDTO> Items,
        AddressDTO ShippingAddress,
        string DeliveryMethod,
        string OrderStatus,
        DateTimeOffset OrderDate,
        decimal SubTotal,
        decimal Total
        );
}