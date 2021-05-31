using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using UserService.Database.Contexts;
using UserService.Database.Converters;
using UserService.Database.Models.Dto;
using UserService.Models;
using UserService.Security;

namespace UserService.Services
{
    public class UserModelService : Controller, IUserService
    {
        private readonly UserContext _context;
        private readonly IAuthenticationService _authenticationService;
        private readonly IDtoConverter<User, UserRequest, UserResponse> _converter;

        public UserModelService(UserContext context,
               IAuthenticationService authenticationService,
            IDtoConverter<User, UserRequest, UserResponse> converter)
        {
            _context = context;
            _authenticationService = authenticationService;
            _converter = converter;
        }

        public async Task<ActionResult<AuthenticationResponse>> AddAsync(UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ValidationState);
            }

            User user = await _context.Users.FirstOrDefaultAsync(e => e.Email == request.Email);

            if (user != null)
            {
                return BadRequest("This email is already in use.");
            }

            User registration = _converter.DtoToModel(request);
            await _context.AddAsync(registration);
            await _context.SaveChangesAsync();

            return await _authenticationService.AuthenticateAsync(new AuthenticationRequest(request));
        }

        public async Task<ActionResult<AuthenticationResponse>> Login(AuthenticationRequest request)
        {
            return await _authenticationService.AuthenticateAsync(request);
        }

        public async Task<ActionResult<UserResponse>> GetByIdAsync(Guid id)
        {
            User user = await _context.Users.FirstOrDefaultAsync(e => e.Id == id);

            if (user == null)
            {
                return NotFound($"User with id {id} not found.");
            }

            return _converter.ModelToDto(user);
        }

        public async Task<ActionResult<UserResponse>> GetByEmailAsync(string email)
        {
            User user = await _context.Users.FirstOrDefaultAsync(e => e.Email == email);

            if (user == null)
            {
                return NotFound($"User with email {email} not found.");
            }

            return _converter.ModelToDto(user);
        }

        public async Task<ActionResult<UserResponse>> UpdateAsync(Guid id, UpdateRequest request)
        {
            User user = await _context.Users.FirstOrDefaultAsync(e => e.Id == id);

            if (user == null)
            {
                return NotFound($"User with id {id} not found.");
            }

            user.Id = id;
            _context.Update(user);
            await _context.SaveChangesAsync();

            UserResponse response = _converter.ModelToDto(user);

            return response;
        }

        public async Task<ActionResult<UserResponse>> DeleteByIdAsync(Guid id)
        {
            User user = await _context.Users.FirstOrDefaultAsync(e => e.Id == id);

            if (user == null)
            {
                throw new Exception($"User with id {id} not found.");
            }

            _context.Remove(user);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
