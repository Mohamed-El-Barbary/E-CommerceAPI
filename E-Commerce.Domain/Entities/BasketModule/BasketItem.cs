namespace E_Commerce.Domain.Entities.BasketModule;

public class BasketItem
{
    
    public int Id { get; set; }
    public string ProductName { get; set; } = default!;
    public string PictureUrl { get; set; } = default!;
    public string ProductSubType { get; set; } = default!;
    public decimal Price { get; set; }
    public string Color { get; set; } = default!;
    public string Size { get; set; } = default!;
    public int Quantity { get; set; }
    
}