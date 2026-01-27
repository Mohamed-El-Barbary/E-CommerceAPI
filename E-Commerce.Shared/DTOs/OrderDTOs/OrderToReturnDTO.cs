namespace E_Commerce.Shared.DTOs.OrderDTOs
{
    public record OrderToReturnDTO
    {
        public Guid Id { get; init; }
        public string UserEmail { get; init; } = default!;
        public string Phone { get; init; } = default!;
        public ICollection<OrderItemDTO> Items { get; init; } = [];
        public AddressDTO Address { get; init; } = default!;
        public string DeliveryMethod { get; init; } = default!;
        public string PaymentIntentId { get; set; } = default!;
        public string OrderStatus { get; init; } = default!;
        public DateTimeOffset OrderDate { get; init; }
        public decimal SubTotal { get; init; }
        public decimal Total { get; init; }
        }
}