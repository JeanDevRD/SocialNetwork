using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Core.Application.DTOs.User;
using SocialNetwork.Core.Application.Interfaces;
using SocialNetwork.Core.Application.ViewModels.User;
using SocialNetwork.Helpers;
using SocialNetwork.Infrastructure.Identity.Entities;

namespace SocialNetwork.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IAccountServiceWeb _accountService;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IMapper _mapper;

        public UserController(IAccountServiceWeb accountService, UserManager<UserEntity> userManager, IMapper mapper)
        {
            _accountService = accountService;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var userSession = await _userManager.GetUserAsync(User);
            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }

            var vm = new EditProfileViewModel
            {
                Id = userSession.Id,
                FirstName = userSession.FirstName,
                LastName = userSession.LastName,
                PhoneNumber = userSession.PhoneNumber ?? "",
                Email = userSession.Email ?? "",
                UserName = userSession.UserName ?? "",
                Password = "",
                ConfirmPassword = ""
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(EditProfileViewModel vm)
        {
            var userSession = await _userManager.GetUserAsync(User);
            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }

            if (!string.IsNullOrWhiteSpace(vm.Password))
            {
                if (vm.Password != vm.ConfirmPassword)
                {
                    ModelState.AddModelError(nameof(vm.ConfirmPassword), "Las contraseñas no coinciden");
                    return View(vm);
                }
            }
            else
            {

                ModelState.Remove(nameof(vm.Password));
                ModelState.Remove(nameof(vm.ConfirmPassword));
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Error al actualizar perfil, los datos son inválidos";
                return View(vm);
            }

            try
            {
                var dto = new CreateUserDto
                {
                    Id = vm.Id,
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    PhoneNumber = vm.PhoneNumber,
                    Email = vm.Email,
                    UserName = vm.UserName,
                    PasswordHash = vm.Password ?? "",
                    Profile = userSession.Profile,
                    IsActive = userSession.IsActive
                };

                string origin = Request?.Headers?.Origin.ToString() ?? string.Empty;

                if (vm.Profile != null)
                {
                    dto.Profile = UploadFile.Uploader(vm.Profile, dto.Id, "Users", true, userSession.Profile);
                }

                var response = await _accountService.EditUser(dto, origin, false);

                if (response.HasError)
                {
                    ViewBag.Error = "Error al actualizar perfil";
                    ViewBag.Errors = response.ErrorMessage;
                    return View(vm);
                }

                ViewBag.Success = "Perfil actualizado exitosamente";

                await _userManager.UpdateSecurityStampAsync(userSession);

                return View(vm);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                ViewBag.Error = "Ha ocurrido un error inesperado al actualizar el perfil";
                return View(vm);
            }
        }
    }
}
