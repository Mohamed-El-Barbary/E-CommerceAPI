namespace E_Commerce.Domain.Entities.BasketModule;

public class CustomerBasket
{
    public string Id { get; set; } = default!; // GUID : Created from Client [Front-End]
    public int NumOfCartItems { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal ShippingPrice { get; set; }
    public int? DeliveryMethodId { get; set; }
    public string? PaymentIntentId { get; set; }
    public string? ClientSecret { get; set; }
    public ICollection<BasketItem> Items { get; set; } = [];
}