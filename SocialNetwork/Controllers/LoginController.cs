using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Core.Application.DTOs.User;
using SocialNetwork.Core.Application.ViewModels.User;
using SocialNetwork.Helpers;
using SocialNetwork_Infrastructure.Identity.Entities;
using SocialNetwork_Infrastructure.Identity.Services;
using System.Data;

namespace SocialNetwork.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAccountServiceWeb _accountServiceWeb;
        private readonly IMapper _mapper;
        private readonly UserManager<UserEntity> _userManager;
        public LoginController(IAccountServiceWeb accountServiceWeb, IMapper mapper, UserManager<UserEntity> userManager)
        {
            _accountServiceWeb = accountServiceWeb;
            _mapper = mapper;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Index(UserLoginDto userLoginDto)
        {
            UserEntity? userSession = await _userManager.GetUserAsync(User);

            if (userSession != null)
            {

                return RedirectToRoute(new { controller = "Home", action = "Index"});
            }

            return View(new UserLoginViewModel() { Password = "", Email = "" });
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserLoginViewModel vm)
        {
            UserEntity? userSession = await _userManager.GetUserAsync(User);

            if (userSession != null)
            {
                var user = await _accountServiceWeb.GetUserByUserName(userSession.UserName ?? "");

                if (user != null )
                {
                    return RedirectToRoute(new { controller = "Home", action = "Index" });
                }
            }

            if (!ModelState.IsValid)
            {
                vm.Password = "";
                return View(vm);
            }

            LoginResponseDto? userDto = await _accountServiceWeb.AuthenticateAsync(new UserLoginDto()
            {
                Password = vm.Password,
                Email = vm.Email
            });

            if (userDto != null && !userDto.HasError)
            {

                return RedirectToRoute(new { controller = "Home", action = "Index" });

            }
            else
            {
                foreach (var error in userDto?.ErrorMessage ?? new List<string>())
                {
                    ModelState.AddModelError("userValidation", error);
                }
            }

            vm.Password = "";
            return View(vm);
        }
        public async Task<IActionResult> Logout()
        {
            await _accountServiceWeb.LogoutAsync();
            return RedirectToRoute(new { controller = "Login", action = "Index" });
        }
        public IActionResult Register()
        {
            return View(new UserRegisterViewModel()
            {
                ConfirmPassword = "",
                Email = "",
                LastName = "",
                FirstName = "",
                PasswordHash = "",
                UserName = "",
                PhoneNumber = "",
                Profile = null!
            });
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            if (vm.PasswordHash != vm.ConfirmPassword)
            {
                ModelState.AddModelError(nameof(vm.ConfirmPassword), "Las contraseñas no coinciden.");
                return View(vm);
            }
            CreateUserDto userDto = _mapper.Map<CreateUserDto>(vm);
            string origin = Request?.Headers?.Origin.ToString() ?? string.Empty;

            RegisterResponseDto? response = await _accountServiceWeb.RegisterUser(userDto, origin);

            if (response.HasError)
            {
                ViewBag.HasError = true;
                ViewBag.Errors = response.ErrorMessage;
                return View(vm);
            }

            if (response != null && !string.IsNullOrWhiteSpace(response.Id))
            {
                userDto.Id = response.Id;
                if (vm.Profile != null)
                {
                    userDto.Profile = UploadFile.Uploader(vm.Profile, userDto.Id, "Users");
                }
                else
                {
                    userDto.Profile = "Images/DefaultProfile.png";
                }
                await _accountServiceWeb.EditUser(userDto , origin, true);
            }

            return RedirectToRoute(new { controller = "Login", action = "Index" });
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            string response = await _accountServiceWeb.ConfirmationAccountAsync(userId, token);
            return View("ConfirmEmail", response);
        }

        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordRequestViewModel() { UserName = "" });
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            string origin = Request?.Headers?.Origin.ToString() ?? string.Empty;

            ForgotPasswordRequestDto dto = new() { UserName = vm.UserName, Origin = origin };

            UserResponseDto? returnUser = await _accountServiceWeb.ForgotPasswordAsync(dto);

            if (returnUser.HasError)
            {
                ViewBag.HasError = true;
                ViewBag.Errors = returnUser.ErrorMessage;
                return View(vm);
            }

            return RedirectToRoute(new { controller = "Login", action = "Index" });
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            ResetPasswordRequestDto dto = _mapper.Map<ResetPasswordRequestDto>(vm);

            UserResponseDto? returnUser = await _accountServiceWeb.ResetPasswordAsync(dto);

            if (returnUser.HasError)
            {
                ViewBag.HasError = true;
                ViewBag.Errors = returnUser.ErrorMessage;
                return View(vm);
            }

            return RedirectToRoute(new { controller = "Login", action = "Index" });
        }

        public async Task<IActionResult> AccessDenied()
        {
            UserEntity? userSession = await _userManager.GetUserAsync(User);

            if (userSession != null)
            {
                var user = await _accountServiceWeb.GetUserByUserName(userSession.UserName!);
                return View();
            }

            return RedirectToRoute(new { controller = "Login", action = "Index" });
        }
    }
}
