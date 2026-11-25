using System.ComponentModel.DataAnnotations;

namespace Railbite.Application.Auth.Dtos;

public class RegisterRequest
{
    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(30)]
    public string Phone { get; set; } = string.Empty;

    [Required, MinLength(6), MaxLength(100)]
    public string Password { get; set; } = string.Empty;

    [MaxLength(300)]
    public string? Address { get; set; }
}
