using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Core.Application.DTOs.Post;
using SocialNetwork.Core.Application.Interfaces;
using SocialNetwork.Core.Application.ViewModels.Post;
using SocialNetwork.Helpers;
using SocialNetwork.Infrastructure.Identity.Entities;

namespace SocialNetwork.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        IPostService _postService;
        IMapper _mapper;
        UserManager<UserEntity> _userManager;
        public PostController(IMapper map, UserManager<UserEntity> userManager, IPostService postService)
        {
            _postService = postService;
            _mapper = map;
            _userManager = userManager;
        }
        public async Task<IActionResult> Create()
        {
            UserEntity? userSession = await _userManager.GetUserAsync(User);

            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }

            return View(new CreatePostViewModel { Id = 0, Content = "", Created = DateTime.Now,
                UserId = "", ImageUrl = null, VideoUrl = "" });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePostViewModel vm)
        {
            UserEntity? userSession = await _userManager.GetUserAsync(User);

            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }
            if (vm.ImageUrl != null && !string.IsNullOrEmpty(vm.VideoUrl))
            {
                ModelState.AddModelError("", "Solo puedes subir una imagen o un video, no ambos.");
                return View(vm);
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Error al Crear publicación, los datos son inválidos";
                return View(vm);
            }

            if (!string.IsNullOrEmpty(vm.VideoUrl))
            {
                if (!vm.VideoUrl.Contains("youtube.com") && !vm.VideoUrl.Contains("youtu.be"))
                {
                    ModelState.AddModelError("VideoUrl", "Solo se permiten enlaces de YouTube");
                    return View(vm);
                }
            }
            try
            {
                vm.UserId = userSession.Id;

                var postDto = _mapper.Map<PostDto>(vm);
                await _postService.AddAsync(postDto);

                if (vm.ImageUrl != null) 
                {
                    postDto.ImageUrl = UploadFile.Uploader(vm.ImageUrl, postDto.Id.ToString(), "Posts");
                    await _postService.UpdateAsync(postDto.Id,postDto);
                    return RedirectToRoute(new { controller = "Home", action = "Index" });

                }

                return RedirectToRoute(new { controller = "Home", action = "Index" });

            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                ViewBag.Error = "Ha ocurrido un error inesperado al crear publicación";
                return View(vm);
            }

        }


        public async Task<IActionResult> Edit(int id)
        {
            UserEntity? userSession = await _userManager.GetUserAsync(User);

            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }

            var postDto = await _postService.GetByIdAsync(id);
            if (postDto == null)
            {
                ViewBag.Error = "No se encontró la publicación vuelva a " +
                    "intentarlo mas tarde o contacta con el soporte";

                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            if (postDto.UserId != userSession.Id)
            {
                ViewBag.Error = "No puedes editar una publicación de otro usuario";

                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            var postVM = _mapper.Map<EditPostViewModel>(postDto);


            return View(postVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditPostViewModel vm)
        {
            UserEntity? userSession = await _userManager.GetUserAsync(User);

            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Error al editar publicación, los datos son inválidos";
                return View(vm);
            }

            try
            {
                var postDto = _mapper.Map<PostDto>(vm);
                await _postService.UpdateAsync(postDto.Id, postDto);

                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                ViewBag.Error = "Ha ocurrido un error inesperado al editar publicación n";
                return View(vm);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            UserEntity? userSession = await _userManager.GetUserAsync(User);
            var dto = await _postService.GetByIdAsync(id);

            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }
            if (dto.UserId != userSession.Id)
            {
                ViewBag.Error = "No puedes editar una publicación de otro usuario";

                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            return View(new DeletePostViewModel() { PostId = id});
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeletePostViewModel vm)
        {
            UserEntity? userSession = await _userManager.GetUserAsync(User);

            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }
            var result = await _postService.DeleteAsync(vm.PostId);
            if (!result) 
            {
                ViewBag.Error = "Error al eliminar publicación, inténtalo de nuevo mas tarde" +
                    "o contacta con el soporte";
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
    }
}
