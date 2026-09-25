using Microsoft.AspNetCore.Mvc;
using CollabWorkspace.Infrastructure.Authentication;
using CollabWorkspace.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using CollabWorkspace.Api.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
namespace CollabWorkspace.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(ITokenService tokenService, UserManager<ApplicationUser> userManager)
        {
            _tokenService= tokenService;
            _userManager= userManager;
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
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
           var user= await _userManager.FindByNameAsync(request.Username);
           if(user is null)
            {
                 return Unauthorized();
            }

            var passwordValid= await _userManager.CheckPasswordAsync(user, request.Password);

            if(!passwordValid)
            {
                return Unauthorized();
            }

            var token= _tokenService.GenerateToken(user);

            return Ok(new { token });
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
          return Ok(new{
                Username = User.FindFirst(JwtRegisteredClaimNames.UniqueName)?.Value,
                isAuthenticated= User.Identity?.IsAuthenticated
            });
        } 


        
    }
}