using E_Commerce.Shared.DTOs.OrderDTOs;

namespace E_Commerce.Shared.DTOs.OrderDTOs
{
    public record OrderDTO(
        string BasketId,
        int DeliveryMethodId,
        string Phone,
        AddressDTO Address
    );
}

