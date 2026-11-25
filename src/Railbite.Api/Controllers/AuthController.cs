using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Railbite.Application.Auth;
using Railbite.Application.Auth.Dtos;
using Railbite.Application.Common.Exceptions;

namespace Railbite.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth)
    {
        _auth = auth;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request, CancellationToken ct)
    {
        var response = await _auth.RegisterAsync(request, ct);
        return Ok(response);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request, CancellationToken ct)
    {
        var response = await _auth.LoginAsync(request, ct);
        return Ok(response);
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<ActionResult<UserProfileDto>> Profile(CancellationToken ct)
    {
        var profile = await _auth.GetProfileAsync(GetUserId(), ct);
        return Ok(profile);
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request, CancellationToken ct)
    {
        await _auth.ChangePasswordAsync(GetUserId(), request, ct);
        return NoContent();
    }

    private Guid GetUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(value, out var id))
        {
            throw new UnauthorizedException("Invalid token.");
        }

        return id;
    }
}
