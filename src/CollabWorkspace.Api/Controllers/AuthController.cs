
using CollabWorkspace.Api.DTOs.Auth;
using CollabWorkspace.Infrastructure.Authentication;
using CollabWorkspace.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace CollabWorkspace.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private const string RefreshCookieName = "refreshToken";
    private const string RefreshCookiePath = "/api/auth/refresh";

    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthController(
        ITokenService tokenService,
        IRefreshTokenService refreshTokenService,
        UserManager<ApplicationUser> userManager)
    {
        _tokenService = tokenService;
        _refreshTokenService = refreshTokenService;
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var user = new ApplicationUser
        {
            UserName = request.Username,
            Email = request.Email,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new { message = "User registered successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null ||
            !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return Unauthorized();
        }

        var accessToken = _tokenService.GenerateToken(user);

        var refreshToken = await _refreshTokenService.CreateAsync(
            user.Id,
            cancellationToken: cancellationToken);

        SetRefreshCookie(
            refreshToken.RawToken,
            refreshToken.ExpiresAtUtc);

        return Ok(new { accessToken });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(
        CancellationToken cancellationToken)
    {
        if (!Request.Cookies.TryGetValue(
                RefreshCookieName, out var rawToken) ||
            string.IsNullOrWhiteSpace(rawToken))
        {
            return Unauthorized();
        }

        var rotation = await _refreshTokenService.RotateAsync(
            rawToken,
            cancellationToken);

        if (rotation is null)
        {
            DeleteRefreshCookie();
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(
            rotation.UserId.ToString());

        if (user is null)
        {
            DeleteRefreshCookie();
            return Unauthorized();
        }

        var accessToken = _tokenService.GenerateToken(user);

        SetRefreshCookie(rotation.RawToken, rotation.ExpiresAtUtc);

        return Ok(new { accessToken });
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            Username = User.FindFirst(
                JwtRegisteredClaimNames.UniqueName)?.Value,
            isAuthenticated = User.Identity?.IsAuthenticated
        });
    }

    private void SetRefreshCookie(string token, DateTime expiresAtUtc)
    {
        Response.Cookies.Append(
            RefreshCookieName,
            token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Path = RefreshCookiePath,
                Expires = new DateTimeOffset(
                    DateTime.SpecifyKind(expiresAtUtc, DateTimeKind.Utc)),
                IsEssential = true
            });
    }

    private void DeleteRefreshCookie()
    {
        Response.Cookies.Delete(
            RefreshCookieName,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Path = RefreshCookiePath
            });
    }
}
