using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Converters;
using UserService.Database.Contexts;
using UserService.Database.Models;
using UserService.Database.Models.Dto;
using UserService.Helpers.Converters;
using UserService.Helpers.Security;
using UserService.Helpers.Security.Models;
using UserService.Models;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly UserContext _context;
        private readonly RegisterDtoConverter _converter;
        private readonly AuthenticationDtoConverter authConverter;
        private readonly IAuthenticationService _authService;

        public UserController(UserContext context, IAuthenticationService authService)
        {
            _context = context;
            _authService = authService;
            _converter = new();
            authConverter = new();
        }

        [HttpPost]
        public async Task<ActionResult<AuthenticationResponse>> Register(RegisterRequest request)
        {
            User user = await FindByEmail(request.Email);

            if (user != null)
            {
                return BadRequest("This email is already in use.");
            }

            User registration = _converter.DtoToModel(request);
            AuthenticationRequest authenticationRequest = authConverter.ModelToDto(request.Email, request.Password);
            await _context.AddAsync(registration);
            await _context.SaveChangesAsync();

            return await _authService.Authenticate(authenticationRequest);
        }

        [HttpGet]
        [Authorize]
        public List<UserResponse> GetAllUsers()
        {
            List<User> registrations = _context.Users.ToList();
            List<UserResponse> users = new();

            foreach (User registration in registrations)
            {
                users.Add(_converter.ModelToDto(registration));
            }

            return users;
        }

        [HttpGet]
        [Authorize]
        [Route("{email}")]
        public async Task<ActionResult<UserResponse>> GetUserByEmail(string email)
        {
            User user = await FindByEmail(email);

            if (user == null)
            {
                return NotFound($"User with email {email} does not exist.");
            }

            return Ok(_converter.ModelToDto(user));
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult> UpdateUserByEmail(string email, UpdateRequest dto)
        {
            User user = await FindByEmail(email);
            UpdateDtoConverter updateConverter = new();

            if (user == null)
            {
                return NotFound($"User with email {email} does not exist.");
            }

            User updatedUser = updateConverter.DtoToModel(dto, user);
            _context.Entry(user).CurrentValues.SetValues(updatedUser);
            _context.SaveChanges();

            return Ok(_converter.ModelToDto(updatedUser));
        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult> DeleteUserById(Guid id)
        {
            User user = await _context.Users.FirstOrDefaultAsync(e => e.Id == id);

            if (user == null)
            {
                return NotFound($"User with id {id} does not exist.");
            }

            _context.Remove(user);
            _context.SaveChanges();

            return Ok();
        }

        private async Task<User> FindByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}