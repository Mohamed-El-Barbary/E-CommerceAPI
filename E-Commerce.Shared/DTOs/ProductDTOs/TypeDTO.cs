namespace E_Commerce.Shared.DTOs.ProductDTOs;

public class TypeDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public ICollection<SubTypesDTO> SubTypes { get; set; } = [];
}