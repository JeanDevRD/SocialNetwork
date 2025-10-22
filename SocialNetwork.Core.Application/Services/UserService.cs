using AutoMapper;
using SocialNetwork.Core.Application.DTOs.Email;
using SocialNetwork.Core.Application.DTOs.User;
using SocialNetwork.Core.Application.Helpers;
using SocialNetwork.Core.Application.Interfaces;
using SocialNetwork.Core.Domain.Entities;
using SocialNetwork.Core.Domain.Interfaces;

namespace SocialNetwork.Core.Application.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IEmailService _emailService;
        private IMapper _mapper;
        public UserService(IUserRepository userRepository, IEmailService emailService, IMapper mapp)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _mapper = mapp;
        }

        public async Task<UserDto?> AddAsync(CreateUserDto dto)
        {
            try
            {
                var user = _mapper.Map<User>(dto);
                var result = await _userRepository.AddAsync(user);

                if (result == null) return null;

                await _emailService.SendAsync(new EmailRequestDto
                      {
                          To = result.Email,
                          Subject = "Bienvenido a la red Social SocialNetwork",
                          BodyHtml = $"<h1>Bienvenido {result.FirstName} {result.LastName}!</h1><p>Gracias por registrarte en SocialNetwork" 
                        
                      });

                return _mapper.Map<UserDto>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);

            if (existingUser == null)  throw new Exception("User not found");

            await _userRepository.DeleteAsync(id);
            return true;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var usersDto = users.Select(user => _mapper.Map<UserDto>(user)).ToList();
            return usersDto;
        }
        public async Task<UserDto?> GetById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null) throw new Exception("User not found");

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> LoginAsync(UserLoginDto dto)

        {
            var user = await _userRepository.GetByEmailAsync(dto.Email, EncryptionPassword.Sha256Hash(dto.Password));
            if (user == null)
            {
                Console.WriteLine($"Usuario no encontrado");
                return null;
            }
            if (!user.IsActive)
            {
                Console.WriteLine($"Usuario inactivo");
                throw new Exception("El usuario está inactivo.");
            }
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> UpdateAsync(CreateUserDto dto)
        {
            var existingUser = await _userRepository.GetByIdAsync(dto.Id);

            if (existingUser == null) throw new Exception("User not found");

            _mapper.Map(dto, existingUser);
            var result = await _userRepository.UpdateAsync(dto.Id, existingUser);
            return _mapper.Map<UserDto>(result);
        }

        public async Task<bool> ChangeStatusAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return false;
            }

            if (user.IsActive == true)
            {
                user.IsActive = false;
            }
            else { user.IsActive = true; }

            await _userRepository.UpdateAsync(id, user);
            return true;

        }
    }
}
