namespace E_Commerce.Shared.DTOs.OrderDTOs
{
    public record AddressDTO
    {
        public string FirstName { get; init; } = default!;
        public string LastName { get; init; } = default!;
        public string City { get; init; } = default!;
        public string Street { get; init; } = default!;
        public string Country { get; init; } = default!;
    }
}