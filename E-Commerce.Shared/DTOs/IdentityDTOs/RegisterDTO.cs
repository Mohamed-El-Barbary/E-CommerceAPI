using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Shared.DTOs.IdentityDTOs;

public record RegisterDTO(
    [EmailAddress] string  Email,
    string DisplayName,
    string UserName,
    string Password,
    [Phone]  string PhoneNumber);
   