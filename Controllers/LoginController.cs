using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using UserService.Database.Contexts;
using UserService.Helpers.Security;
using UserService.Helpers.Security.Models;
using UserService.Models;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class LoginController : Controller
    {
        private readonly IAuthenticationService _authService;
        public LoginController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AuthenticationResponse>> Login(AuthenticationRequest request)
        {
            return await _authService.Authenticate(request);
        }
    }
}
