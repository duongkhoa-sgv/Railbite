using System.ComponentModel.DataAnnotations;

namespace Railbite.Application.Auth.Dtos;

public class ChangePasswordRequest
{
    [Required, MaxLength(100)]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required, MinLength(6), MaxLength(100)]
    public string NewPassword { get; set; } = string.Empty;
}
