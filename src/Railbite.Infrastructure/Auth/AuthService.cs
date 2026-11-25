using Microsoft.EntityFrameworkCore;
using Railbite.Application.Auth;
using Railbite.Application.Auth.Dtos;
using Railbite.Application.Common.Exceptions;
using Railbite.Domain.Entities;
using Railbite.Domain.Enums;
using Railbite.Infrastructure.Persistence;

namespace Railbite.Infrastructure.Auth;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly ITokenService _tokens;

    public AuthService(AppDbContext db, ITokenService tokens)
    {
        _db = db;
        _tokens = tokens;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        if (await _db.Users.AnyAsync(u => u.Email == email, ct))
        {
            throw new ConflictException("Email is already registered.");
        }

        var passenger = new Passenger
        {
            Name = request.Name.Trim(),
            Email = email,
            Phone = request.Phone.Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = UserRole.Passenger,
            Address = string.IsNullOrWhiteSpace(request.Address) ? null : request.Address.Trim()
        };

        passenger.Wallet = new Wallet { PassengerId = passenger.Id, Balance = 0m };
        passenger.Cart = new Cart { PassengerId = passenger.Id };

        _db.Passengers.Add(passenger);
        await _db.SaveChangesAsync(ct);

        var (token, expires) = _tokens.CreateToken(passenger);
        return new AuthResponse
        {
            Token = token,
            ExpiresAtUtc = expires,
            User = MapProfile(passenger, passenger.Wallet.Balance)
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email, ct);

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedException();
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedException("Account is disabled.");
        }

        var (token, expires) = _tokens.CreateToken(user);
        return new AuthResponse
        {
            Token = token,
            ExpiresAtUtc = expires,
            User = MapProfile(user, await GetWalletBalanceAsync(user, ct))
        };
    }

    public async Task<UserProfileDto> GetProfileAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId, ct)
            ?? throw new NotFoundException("User not found.");

        if (!user.IsActive)
        {
            throw new UnauthorizedException("Account is disabled.");
        }

        return MapProfile(user, await GetWalletBalanceAsync(user, ct));
    }

    public async Task ChangePasswordAsync(Guid userId, ChangePasswordRequest request, CancellationToken ct = default)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId, ct)
            ?? throw new NotFoundException("User not found.");

        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
        {
            throw new AppValidationException("Current password is incorrect.");
        }

        if (request.CurrentPassword == request.NewPassword)
        {
            throw new AppValidationException("New password must be different from the current password.");
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        await _db.SaveChangesAsync(ct);
    }

    private async Task<decimal?> GetWalletBalanceAsync(User user, CancellationToken ct)
    {
        if (user is not Passenger passenger)
        {
            return null;
        }

        return await _db.Wallets
            .Where(w => w.PassengerId == passenger.Id)
            .Select(w => (decimal?)w.Balance)
            .FirstOrDefaultAsync(ct) ?? 0m;
    }

    private static UserProfileDto MapProfile(User user, decimal? walletBalance)
    {
        var dto = new UserProfileDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Phone = user.Phone,
            Role = user.Role.ToString(),
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            WalletBalance = walletBalance
        };

        switch (user)
        {
            case Passenger passenger:
                dto.Address = passenger.Address;
                break;
            case Restaurant restaurant:
                dto.Address = restaurant.Address;
                dto.RestaurantName = restaurant.RestaurantName;
                break;
            case DeliveryPerson deliveryPerson:
                dto.VehicleNumber = deliveryPerson.VehicleNumber;
                break;
        }

        return dto;
    }
}
