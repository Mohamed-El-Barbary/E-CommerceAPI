namespace E_Commerce.Shared.DTOs.ProductDTOs;

public class ProductDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string PictureUrl { get; set; } = default!;
    public decimal Price { get; set; }
    public decimal Discount { get; set; } 
    public string ProductBrand { get; set; } = default!;
    public string ProductType { get; set; } = default!;
    public string ProductSubType { get; set; } = default!;
    public string Stock { get; set; } = default!;
    public string SKU { get; set; } = default!;
    public ICollection<ColorDTO> ProductColors { get; set; } = default!;
    public ICollection<string> ProductSizes { get; set; } = default!;
    public ICollection<string>? Images { get; set; } = null; 


}