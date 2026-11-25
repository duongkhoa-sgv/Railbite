using Railbite.Domain.Entities;

namespace Railbite.Application.Auth;

public interface ITokenService
{
    (string Token, DateTime ExpiresAtUtc) CreateToken(User user);
}
