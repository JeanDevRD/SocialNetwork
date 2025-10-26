using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Core.Application.DTOs.Email;
using SocialNetwork.Core.Application.DTOs.Game;
using SocialNetwork.Core.Application.DTOs.User;
using SocialNetwork.Core.Application.Interfaces;
using SocialNetwork_Infrastructure.Identity.Entities;
using System.Text;


namespace SocialNetwork_Infrastructure.Identity.Services
{
    public class AccountServiceWeb : IAccountServiceWeb
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly IEmailService _emailService;
        private IMapper _mapper;
        public AccountServiceWeb(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager,
            IEmailService emailService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task<LoginResponseDto> AuthenticateAsync(UserLoginDto dto)
        {
            LoginResponseDto response = new()
            {
                Id = string.Empty,
                FirstName = string.Empty,
                LastName = string.Empty,
                Email = string.Empty,
                UserName = string.Empty,
                Profile = string.Empty,
                HasError = false,
            };

            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                response.HasError = true;
                if (response.ErrorMessage == null)
                    response.ErrorMessage = new List<string>();
                response.ErrorMessage.Add($"No se ha encontrado ningún usuario con ese correo: {dto.Email}");
                return response;
            }

            if (!user.EmailConfirmed || !user.IsActive)
            {
                response.HasError = true;
                if (response.ErrorMessage == null)
                    response.ErrorMessage = new List<string>();
                response.ErrorMessage.Add($"La cuenta:{user.UserName} está inactiva");
                return response;
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user.UserName!, dto.Password, false, lockoutOnFailure: true);

            if (!signInResult.Succeeded)
            {
                response.HasError = true;
                if (response.ErrorMessage == null)
                    response.ErrorMessage = new List<string>();
                response.ErrorMessage.Add($"Credenciales incorrectas para el usuario: {user.UserName}");
                return response;
            }

            response = _mapper.Map(user, response);

            return response;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<RegisterResponseDto> RegisterUser(CreateUserDto dto, string origin)
        {

            RegisterResponseDto response = new()
            {
                FirstName = "",
                LastName = "",
                Email = "",
                Id = "",
                UserName = "",
                Profile = "",
                HasError = false
            };
            var userExists = await _userManager.FindByNameAsync(dto.UserName);
            if (userExists != null)
            {
                response.HasError = true;
                response.ErrorMessage = $"El usuario con el nombre: {dto.UserName} ya existe.";
                return response;
            }
            userExists = await _userManager.FindByEmailAsync(dto.Email);
            if (userExists != null)
            {
                response.HasError = true;
                response.ErrorMessage = $"El usuario con el correo: {dto.Email} ya existe.";
                return response;
            }

            UserEntity user = new UserEntity()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.UserName,
                Profile = string.IsNullOrWhiteSpace(dto.Profile) ? "Images/default_profile.png" : dto.Profile,
                EmailConfirmed = false,
                PhoneNumber = dto.PhoneNumber,
                IsActive = dto.IsActive
            };


            var result = await _userManager.CreateAsync(user, dto.PasswordHash);


            if (!result.Succeeded)
            {
                response.HasError = true;
                response.ErrorMessage = $"El usuario con el correo: {dto.Email} ya existe.";
                return response;
            }

            string verificationUri = await GetEmailVerificationUri(user, origin);
            await _emailService.SendAsync(new EmailRequestDto()
            {
                To = dto.Email,
                Subject = "Confirmar verificación",
                BodyHtml = $"Por favor, confirma tu cuenta visitando el Url: {verificationUri}"
            });

            return response;

        }
        public async Task<EditResponseDto> EditUser(CreateUserDto dto, string origin, bool? isCreated = false)
        {
            bool isNotcreated = !isCreated ?? false;

            EditResponseDto response = new()
            {
                FirstName = "",
                LastName = "",
                Email = "",
                Id = "",
                UserName = "",
                Profile = null!,
                HasError = false
            };
            var userExists = await _userManager.FindByNameAsync(dto.UserName);
            if (userExists != null)
            {
                response.HasError = true;
                response.ErrorMessage.Add($"El usuario con el nombre: {dto.UserName} ya existe.");
                return response;
            }
            userExists = await _userManager.FindByEmailAsync(dto.Email);
            if (userExists != null)
            {
                response.HasError = true;
                response.ErrorMessage.Add($"El usuario con el correo: {dto.Email} ya existe.");
                return response;
            }

            var user = await _userManager.FindByIdAsync(dto.Id);

            if (user == null)
            {
                response.HasError = true;
                response.ErrorMessage.Add($"Esta cuenta no está registrada");
                return response;
            }

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            user.UserName = dto.UserName;
            user.PhoneNumber = dto.PhoneNumber;
            user.Profile = string.IsNullOrWhiteSpace(dto.Profile) ? user.Profile : dto.Profile;
            user.EmailConfirmed = user.EmailConfirmed && user.Email == dto.Email;

            if (!string.IsNullOrWhiteSpace(dto.PasswordHash) && isNotcreated)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resultChange = await _userManager.ResetPasswordAsync(user, token, dto.PasswordHash);

                if (resultChange != null && !resultChange.Succeeded)
                {
                    response.HasError = true;
                    response.ErrorMessage.AddRange(resultChange.Errors.Select(s => s.Description).ToList());
                    return response;
                }
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.ErrorMessage.Add($"El usuario con el correo: {dto.Email} ya existe.");
                return response;
            }
            if (!user.EmailConfirmed)
            {
                string verificationUri = await GetEmailVerificationUri(user, origin);
                await _emailService.SendAsync(new EmailRequestDto()
                {
                    To = dto.Email,
                    Subject = "Confirmar verificación",
                    BodyHtml = $"Por favor, confirma tu cuenta visitando el Url: {verificationUri}"
                });
            }

            if (!string.IsNullOrEmpty(dto.PasswordHash))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _userManager.ResetPasswordAsync(user, token, dto.PasswordHash);
            }
            response = _mapper.Map(user, response);
            return response;
        }
        public async Task<UserResponseDto> ForgotPasswordAsync(ForgotPasswordRequestDto dto)
        {
            UserResponseDto response = new()
            {
                HasError = false
            };
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null)
            {
                response.HasError = true;
                response.ErrorMessage = $"Esta cuenta no está registrada";
                return response;
            }

            var resetUri = await GetResetPasswordUri(user, dto.Origin);
            user.EmailConfirmed = false;
            await _userManager.UpdateAsync(user);
            await _emailService.SendAsync(new EmailRequestDto()
            {
                To = user.Email,
                Subject = "Confirmar verificación",
                BodyHtml = $"Resetea tu contraseña el Url: {resetUri}"
            });

            return response;
        }
        public async Task<UserResponseDto> ResetPasswordAsync(ResetPasswordRequestDto request)
        {
            UserResponseDto response = new()
            {
                HasError = false
            };
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null)
            {
                response.HasError = true;
                response.ErrorMessage = $"Esta cuenta no está registrada";
                return response;
            }

            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            var result = await _userManager.ResetPasswordAsync(user, token, request.Password);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.ErrorMessage = $"Error al resetear la contraseña";
                return response;
            }
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);
            return response;
        }
        public async Task<UserResponseDto> DeleteAsync(string userId)
        {
            UserResponseDto response = new()
            {
                HasError = false
            };
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                response.HasError = true;
                response.ErrorMessage = $"Esta cuenta no está registrada";
                return response;
            }

            await _userManager.DeleteAsync(user);
            return response;
        }
        public async Task<UserDto> GetUserByEmail(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);

            if (user == null)
            {
                return null!;
            }

            var userDto = new UserDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.PhoneNumber!,
                Email = user.Email!,
                Username = user.UserName!,
                Profile = user.Profile,
                IsActive = user.IsActive,
                Id = user.Id,
                IsVerified = user.EmailConfirmed
            };

            return userDto;
        }
        public async Task<UserDto> GetUserByUserName(string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);

            if (user == null)
            {
                return null!;
            }

            var userDto = new UserDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.PhoneNumber!,
                Email = user.Email!,
                Username = user.UserName!,
                Profile = user.Profile,
                IsActive = user.IsActive,
                Id = user.Id,
                IsVerified = user.EmailConfirmed
            };

            return userDto;
        }
        public async Task<List<UserDto>> GetAllUser(bool? isActive = true)
        {
            var user = _userManager.Users.Select(s => new UserDto
            {
                FirstName = s.FirstName,
                LastName = s.LastName,
                Phone = s.PhoneNumber!,
                Email = s.Email!,
                Username = s.UserName!,
                Profile = s.Profile,
                IsActive = s.IsActive,
                Id = s.Id,
                IsVerified = s.EmailConfirmed
            });

            if (isActive == true && isActive != null)
            {
                user = user.Where(w => w.IsVerified);
            }
            else
            {
                user = user.Where(w => w.IsVerified == false);
            }
            return await user.ToListAsync();
        }
        public async Task<string> ConfirmationAccountAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return "El usuario no existe";
            }
            if (string.IsNullOrWhiteSpace(token))
            {
                return "El token de confirmación no fue proporcionado o está vacío.";
            }

            try
            {
                var decoded = WebEncoders.Base64UrlDecode(token);
                var code = Encoding.UTF8.GetString(decoded);
                var result = await _userManager.ConfirmEmailAsync(user, code);

                if (!user.IsActive)
                {
                    user.IsActive = true;
                    await _userManager.UpdateAsync(user);
                }

                return result.Succeeded ? $"La cuenta ha sido confirmada para {user.Email}"
                                        : $"Error al confirmar la cuenta {user.Email}";

            }
            catch (FormatException)
            {
                return "El token no tiene un formato válido.";
            }
        }
        #region private methods
        private async Task<string> GetEmailVerificationUri(UserEntity user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "Login/ConfirmEmail";
            var Uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "userId", user.Id);
           
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "token", code);
            return verificationUri;
        }

        private async Task<string> GetResetPasswordUri(UserEntity user, string origin)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "Login/ResetPassword";
            var Uri = new Uri(string.Concat($"{origin}/", route));
            var resetUri = QueryHelpers.AddQueryString(Uri.ToString(), "userId", user.Id);
            resetUri = QueryHelpers.AddQueryString(resetUri, "token", code);
            return resetUri;
        }
        #endregion
    }


}
