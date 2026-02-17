namespace E_Commerce.Shared.DTOs.BasketDTOs;

public record BasketDTO
{
    public string Id { get; set; } = default!;
    public int? DeliveryMethodId { get; set; }
    public decimal ShippingPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public int NumOfCartItems { get; set; }
    public string? PaymentIntentID { get; set; }
    public string? ClientSecret { get; set; }
    public ICollection<BasketItemDTO> Items { get; set; } = [];

};