using E_Commerce.Shared.DTOs.OrderDTOs;

namespace E_Commerce.Shared.DTOs.OrderDTOs
{
    public record OrderDTO
    {

        public string BasketId { get; init; } = default!;

        public int DeliveryMethodId { get; init; }

        public string Phone { get; init; } = default!;

        public AddressDTO Address { get; init; } = default!;
    }
}

