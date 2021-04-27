using System;
using System.Threading.Tasks;
using UserService.Database.Converters;
using UserService.Database.Models.Dto;
using UserService.Models;
using UserService.Repositories;

namespace UserService.Services
{
    public class UserModelService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IDtoConverter<User, UserRequest, UserResponse> _converter;
        private readonly UpdateDtoConverter _updateDtoConverter;

        public UserModelService(IUserRepository repository, IDtoConverter<User, UserRequest, UserResponse> converter)
        {
            _repository = repository;
            _converter = converter;
            _updateDtoConverter = new();
        }

        public async Task<User> AddUserAsync(UserRequest request)
        {
            User user = await _repository.GetByEmailAsync(request.Email);

            if (user != null)
            {
                throw new Exception("This email is already in use.");
            }

            User registration = _converter.DtoToModel(request);

            return await _repository.AddAsync(registration);
        }

        public async Task<User> DeleteUserByIdAsync(Guid id)
        {
            User user = await _repository.GetByIdAsync(id);

            if (user == null)
            {
                throw new Exception($"User with id {id} not found.");
            }

            return await _repository.DeleteAsync(await _repository.GetByIdAsync(id));
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            User user = await _repository.GetByEmailAsync(email);

            if (user != null)
            {
                throw new Exception($"User with email {email} not found.");
            }

            return await _repository.GetByEmailAsync(email);
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            User user = await _repository.GetByIdAsync(id);

            if (user == null)
            {
                throw new Exception($"User with id {id} not found.");
            }

            return user;
        }

        public async Task<User> UpdateUserAsync(Guid id, UpdateRequest request)
        {
            User user = await _repository.GetByIdAsync(id);

            if (user != null)
            {
                throw new Exception($"User with id {id} not found.");
            }

            User updatedUser = _updateDtoConverter.DtoToModel(request, user);

            return await _repository.UpdateAsync(updatedUser);
        }
    }
}
