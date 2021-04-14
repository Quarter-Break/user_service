using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly AuthenticationRequestConverter authenticationRequestConverter;
        private readonly IAuthenticationService _authService;

        public UserController(UserContext context, IAuthenticationService authService)
        {
            _context = context;
            _authService = authService;
            _converter = new();
            authenticationRequestConverter = new();
        }

        [HttpPost]
        public IActionResult Register(RegisterDto dto)
        {
            User registration = _converter.DtoToModel(dto);

            if (ModelState.IsValid)
            {
                _context.Add(registration);
                _context.SaveChanges();

                return _authService.Authenticate(authenticationRequestConverter.DtoToRequest(dto.Email, dto.Password));
            }

            return BadRequest();
        }

        [HttpGet]
        [Authorize]
        public List<UserDto> GetAllUsers()
        {
            List<User> registrations = _context.Users.ToList();
            List<UserDto> users = new();

            foreach (User registration in registrations)
            {
                users.Add(_converter.ModelToDto(registration));
            }

            return users;
        }

        [HttpGet]
        [Authorize]
        [Route("{email}")]
        public ActionResult<UserDto> GetUserByEmail(string email)
        {
            User user = FindByEmail(email);

            if (user != null)
            {
                return Ok(_converter.ModelToDto(user));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        [Authorize]
        public IActionResult UpdateUserByEmail(string email, UpdateDto dto)
        {
            User user = FindByEmail(email);
            UpdateDtoConverter updateConverter = new();

            if (user != null)
            {
                User updatedUser = updateConverter.DtoToModel(dto, user);
                _context.Entry(user).CurrentValues.SetValues(updatedUser);
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [Authorize]
        public IActionResult DeleteUserById(Guid id)
        {
            User user = new() { Id = id };

            _context.Users.Attach(user);
            _context.Remove(user);
            _context.SaveChanges();

            return Ok();
        }

        private User FindByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }
    }
}
