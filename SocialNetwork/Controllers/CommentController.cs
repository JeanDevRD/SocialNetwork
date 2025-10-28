using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Core.Application.DTOs.Comment;
using SocialNetwork.Core.Application.Interfaces;
using SocialNetwork.Core.Application.ViewModels.Comment;
using SocialNetwork.Infrastructure.Identity.Entities;

namespace SocialNetwork.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        IMapper _autoMapper;
        UserManager<UserEntity> _userManager;
        ICommentService _commentService;
        public CommentController(IMapper autoMapper, UserManager<UserEntity> userManager, ICommentService commentService)
        {
            _autoMapper = autoMapper;
            _userManager = userManager;
            _commentService = commentService;
        }

        public async Task<IActionResult> Create(int id)
        {
            UserEntity? userSession = await _userManager.GetUserAsync(User);
            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }
            return View(new CreateCommentViewModel { Id = 0, Content = "", Created = DateTime.Now,
                UserId = "", PostId = id });

        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCommentViewModel vm)
        {
            UserEntity? userSession = await _userManager.GetUserAsync(User);
            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Error al Crear comentario, los datos son inválidos";
                return View(vm);
            }
            try
            {
                vm.Id = 0;
                vm.UserId = userSession.Id;
                var commentDto = _autoMapper.Map<CommentDto>(vm);
                await _commentService.AddAsync(commentDto);
                return RedirectToRoute(new { controller = "home", action = "Index" });
            }
            catch (Exception)
            {
                ViewBag.Error = "No se ha podido crear el comentario";
                return View(vm);
            }
        }
        public async Task<IActionResult>CreateReply(int id)
        {
            UserEntity? userSession = await _userManager.GetUserAsync(User);
            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }
            var parentComment = await _commentService.GetByIdAsync(id);
            if (parentComment == null)
            {
                ViewBag.Error = "No se encontró el comentario al que deseas responder";
                return RedirectToRoute(new { controller = "home", action = "Index" });
            }
            return View( new CreateCommentViewModel 
            { 
                Id = 0, 
                Content = "", 
                Created = DateTime.Now,
                UserId = "", 
                PostId = parentComment.PostId,
                ParentCommentId = parentComment.Id
            });
        }
        [HttpPost]
        public async Task<IActionResult>CreateReply(CreateCommentViewModel vm)
        {
            UserEntity? userSession = await _userManager.GetUserAsync(User);
            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Error al Crear comentario, los datos son inválidos";
                return View(vm);
            }
            try
            {
                vm.UserId = userSession.Id;
                var commentDto = _autoMapper.Map<CommentDto>(vm);
                await _commentService.AddAsync(commentDto);
                return RedirectToRoute(new { controller = "home", action = "Index" });
            }
            catch (Exception)
            {
                ViewBag.Error = "No se ha podido crear el comentario";
                return View(vm);
            }
        }
        public async Task<IActionResult> Edit(int id)
        {
            UserEntity? userSession = await _userManager.GetUserAsync(User);
            var commentDto = await _commentService.GetByIdAsync(id);
            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }
            if (commentDto.UserId != userSession.Id)
            {
                ViewBag.Error = "No puedes editar un comentario de otro usuario";
                return RedirectToRoute(new { controller = "home", action = "Index" });
            }
            var vm = _autoMapper.Map<EditCommentViewModel>(commentDto);
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditCommentViewModel vm)
        {
            UserEntity? userSession = await _userManager.GetUserAsync(User);
            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Error al editar comentario, los datos son inválidos";
                return View(vm);
            }
            var commentDto = await _commentService.GetByIdAsync(vm.CommentId);
            if (commentDto.UserId != userSession.Id)
            {
                ViewBag.Error = "No puedes editar un comentario de otro usuario";
                return RedirectToRoute(new { controller = "home", action = "Index" });
            }
            try
            {
                commentDto.Content = vm.Content;
                var result = await _commentService.UpdateAsync(commentDto.Id,commentDto);
                if (result != null)
                {
                    return RedirectToRoute(new { controller = "home", action = "Index" });
                }
                else
                {
                    ViewBag.Error = "No se ha podido editar el comentario";
                    return View(vm);
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "No se ha podido editar el comentario";
                return View(vm);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            UserEntity? userSession = await _userManager.GetUserAsync(User);
            var commentDto = await _commentService.GetByIdAsync(id);
            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }
            if (commentDto.UserId != userSession.Id)
            {
                ViewBag.Error = "No puedes eliminar un comentario de otro usuario";
                return RedirectToRoute(new { controller = "home", action = "Index" });
            }
            return View(_autoMapper.Map<DeleteCommentViewModel>(commentDto));

        }
        [HttpPost]
        public async Task<IActionResult> Delete(DeleteCommentViewModel vm)
        {
            UserEntity? userSession = await _userManager.GetUserAsync(User);
            var commentDto = await _commentService.GetByIdAsync(vm.CommentId);
            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }

            if (commentDto.UserId != userSession.Id)
            {
                ViewBag.Error = "No puedes eliminar un comentario de otro usuario";
                return RedirectToRoute(new { controller = "home", action = "Index" });
            }
            try
            {
               var result = await _commentService.DeleteAsync(vm.CommentId);
                if (!result) 
                { 
                    ViewBag.Error = "No se ha podido eliminar el comentario";
                }
                return RedirectToRoute(new { controller = "home", action = "Index" });
            }
            catch (Exception)
            {
                ViewBag.Error = "No se ha podido eliminar el comentario";
                return RedirectToRoute(new { controller = "home", action = "Index" });
            }

        }
    }
}
