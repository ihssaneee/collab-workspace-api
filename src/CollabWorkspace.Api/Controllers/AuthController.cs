using Microsoft.AspNetCore.Mvc;
using CollabWorkspace.Infrastructure.Authentication;
using CollabWorkspace.Infrastructure.Data;
using CollabWorkspace.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

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


        
    }
}