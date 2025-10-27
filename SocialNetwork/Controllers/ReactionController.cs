using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Core.Application.DTOs.Reaction;
using SocialNetwork.Core.Application.Interfaces;
using SocialNetwork.Core.Application.ViewModels.Reaction;
using SocialNetwork.Infrastructure.Identity.Entities;

namespace SocialNetwork.Controllers
{
   [Authorize]
    public class ReactionController : Controller
    {

        IReactionService _reactionService;
        IMapper _mapper;
        UserManager<UserEntity> _userManager;
        public ReactionController(IMapper mapper, UserManager<UserEntity> userManager, IReactionService reactionService)
        {
            _reactionService = reactionService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create(int id)
        {
            UserEntity? userSession = await _userManager.GetUserAsync(User);

            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Ha ocurrido un error al reaccionar a esta publicación";
                return RedirectToRoute(new { controller = "home", action = "Index" });

            }
            try
            {
                var vm = new ReactionViewModel
                {
                    Id = 0,
                    Type = "Like",
                    Created = DateTime.Now,
                    UserId = userSession.Id,
                    PostId = id
                };
                var reactionDto = _mapper.Map<ReactionDto>(vm);
                var result = _reactionService.AddAsync(reactionDto);
                if (result != null)
                {
                    return RedirectToRoute(new { controller = "home", action = "Index" });
                }
                else
                {
                    ViewBag.Error = "No se ha podido reaccionar a esta publicación";
                    return RedirectToRoute(new { controller = "home", action = "Index" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                ViewBag.Error = "Ha ocurrido un error inesperado al reaccionar a esta publicación";
                return RedirectToRoute(new { controller = "home", action = "Index" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            UserEntity? userSession = await _userManager.GetUserAsync(User);
            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Ha ocurrido un error al eliminar la reacción a esta publicación";
                return RedirectToRoute(new { controller = "home", action = "Index" });
            }
            try
            {
                var result = await _reactionService.DeleteAsync(id);
                if (result)
                {
                    return RedirectToRoute(new { controller = "home", action = "Index" });
                }
                else 
                {
                    ViewBag.Error = "No se ha podido eliminar la reacción a esta publicación";
                    return RedirectToRoute(new { controller = "home", action = "Index" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                ViewBag.Error = "Ha ocurrido un error inesperado al eliminar la reacción a esta publicación";
                return RedirectToRoute(new { controller = "home", action = "Index" });
            }
        }
    }
}
