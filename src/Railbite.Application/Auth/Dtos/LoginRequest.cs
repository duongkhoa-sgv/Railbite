using System.ComponentModel.DataAnnotations;

namespace Railbite.Application.Auth.Dtos;

public class LoginRequest
{
    [Required, EmailAddress, MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Password { get; set; } = string.Empty;
}
