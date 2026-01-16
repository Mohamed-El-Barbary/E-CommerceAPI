using System.Text.Json.Serialization;

namespace E_Commerce.Shared.DTOs.IdentityDTOs
{
    public record UserDTO(
        string  Email, 
        string DisplayName, 
        string AccessToken,
        [property: JsonIgnore]
        string RefershToken, 
        DateTime RefershTokenExpired);
}