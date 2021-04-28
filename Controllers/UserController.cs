using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UserService.Database.Converters;
using UserService.Database.Models.Dto;
using UserService.Models;
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
        private readonly IDtoConverter<User, UserRequest, UserResponse> _converter;

        public UserController(IUserService service,
            IAuthenticationService authenticationService,
           IDtoConverter<User, UserRequest, UserResponse> converter)
        {
            _service = service;
            _authenticationService = authenticationService;
            _converter = converter;
        }

        [HttpPost]
        public async Task<ActionResult<AuthenticationResponse>> AddAsync(UserRequest request)
        {
            await _service.AddUserAsync(request);

            return Created("Created", await _authenticationService.AuthenticateAsync(new AuthenticationRequest(request)));
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
            return _converter.ModelToDto(await _service.GetUserByIdAsync(id));
        }

        [HttpGet]
        [Authorize]
        [Route("email/{email}")]
        public async Task<ActionResult<UserResponse>> GetUserByEmailAsync(string email)
        {
            return _converter.ModelToDto(await _service.GetUserByEmailAsync(email));
        }

        [HttpPut]
        [Authorize]
        [Route("{id}")]
        public async Task<ActionResult<UserResponse>> UpdateUserByIdAsync(Guid id, UpdateRequest dto)
        {
            return _converter.ModelToDto(await _service.UpdateUserAsync(id, dto));
        }

        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public async Task<ActionResult> DeleteUserAsync(Guid id)
        {
            await _service.DeleteUserByIdAsync(id);

            return Ok();
        }
    }
}