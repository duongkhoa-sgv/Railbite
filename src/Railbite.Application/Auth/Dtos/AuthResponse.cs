namespace Railbite.Application.Auth.Dtos;

public class UserProfileDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Address { get; set; }
    public string? RestaurantName { get; set; }
    public string? VehicleNumber { get; set; }
    public decimal? WalletBalance { get; set; }
}

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; set; }
    public UserProfileDto User { get; set; } = new();
}
