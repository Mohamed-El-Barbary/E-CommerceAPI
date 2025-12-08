using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Shared.DTOs.IdentityDTOs
{
    public record LoginDTO([EmailAddress]  string Email ,string Password);
}