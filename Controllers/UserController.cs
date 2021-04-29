using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UserService.Database.Models.Dto;
using UserService.Security;
using UserService.Services;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly IUserService _service;
        private readonly IAuthenticationService _authenticationService;

        public UserController(IUserService service,
            IAuthenticationService authenticationService)
        {
            _service = service;
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<ActionResult<AuthenticationResponse>> AddAsync(UserRequest request)
        {
            return Created("Created", await _service.AddAsync(request));
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AuthenticationResponse>> Login(AuthenticationRequest request)
        {
            return await _authenticationService.AuthenticateAsync(request);
        }

        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public async Task<ActionResult<UserResponse>> GetUserByIdAsync(Guid id)
        {
            return await _service.GetByIdAsync(id);
        }

        [HttpGet]
        [Authorize]
        [Route("email/{email}")]
        public async Task<ActionResult<UserResponse>> GetUserByEmailAsync(string email)
        {
            return await _service.GetByEmailAsync(email);
        }

        [HttpPut]
        [Authorize]
        [Route("{id}")]
        public async Task<ActionResult<UserResponse>> UpdateUserByIdAsync(Guid id, UpdateRequest request)
        {
            return await _service.UpdateAsync(id, request);
        }

        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public async Task<ActionResult<UserResponse>> DeleteUserAsync(Guid id)
        {
            return await _service.DeleteByIdAsync(id);
        }
    }
}