using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Core.Application.Interfaces;
using SocialNetwork.Core.Application.ViewModels.FriendShip;
using SocialNetwork_Infrastructure.Core.Application.Interfaces;
using SocialNetwork_Infrastructure.Identity.Entities;

namespace SocialNetwork.Controllers
{
    public class FriendShipController : Controller
    {
        private readonly IFriendShipService _friendShipService;
        private readonly IPostService _postService;
        private readonly IAccountServiceWeb _accountService;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IMapper _mapper;

        public FriendShipController(
            IFriendShipService friendShipService,
            IPostService postService,
            IAccountServiceWeb accountService,
            UserManager<UserEntity> userManager,
            IMapper mapper)
        {
            _friendShipService = friendShipService;
            _postService = postService;
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
                
            var currentUser = await _accountService.GetUserByUserName(userSession.UserName!);
            var allFriendships = await _friendShipService.GetAllAsync();
            var allPosts = await _postService.GetAllAsync();

            var currentUserId = currentUser.Id;

            var userFriendships = allFriendships
                .Where(f => f.UserId == currentUserId || f.FriendId == currentUserId)
                .ToList();

            var friendIds = userFriendships
                .Select(f => f.UserId == currentUserId ? f.FriendId.ToString() : f.UserId.ToString())
                .Distinct().ToList();

            var friends = new List<FriendViewModel>();
            foreach (var friendId in friendIds)
            {
                var friend = await _accountService.GetUserByUserName((await _userManager.FindByIdAsync(friendId))?.UserName!);
                if (friend != null)
                {
                    friends.Add(new FriendViewModel
                    {
                        Id = friend.Id,
                        FirstName = friend.FirstName,
                        LastName = friend.LastName,
                        Username = friend.Username,
                        Profile = friend.Profile
                    });
                }
            }

            var friendPostsQuery = allPosts.Where(p => friendIds.Contains(p.UserId))
            .OrderByDescending(p => p.Created).ToList();

            var friendPosts = _mapper.Map<List<FriendPostViewModel>>(friendPostsQuery);

            var viewModel = new FriendsIndexViewModel
            {
                Friends = friends,
                FriendPosts = friendPosts
            };

            return View(viewModel);
        }

        public async Task<IActionResult> UserPosts(string userId)
        {
            var userSession = await _userManager.GetUserAsync(User);
            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return RedirectToRoute(new { controller = "Friends", action = "Index" });

            var allPosts = await _postService.GetAllAsync();

            var userPosts = allPosts.Where(p => p.UserId == userId)
                .OrderByDescending(p => p.Created)
                .Select(p => _mapper.Map<FriendPostViewModel>(p))
                .ToList();

            var viewModel = new UserPostsViewModel
            {
                UserId = userId,
                UserName = user.UserName,
                UserProfile = user.Profile,
                Posts = userPosts
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var userSession = await _userManager.GetUserAsync(User);
            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }

            var currentUser = await _accountService.GetUserByUserName(userSession.UserName!);
            var allFriendships = await _friendShipService.GetAllAsync();

            var friendship = allFriendships.FirstOrDefault(f =>
                f.UserId.ToString() == currentUser.Id && f.FriendId.ToString() == id);
                

            if (friendship == null)
                return RedirectToRoute(new { controller = "Friends", action = "Index" });

            var friend = await _accountService.GetUserByUserName((await _userManager.FindByIdAsync(id))?.UserName!);

            var viewModel = new DeleteFriendViewModel
            {
                FriendShipId = friendship.Id,
                FriendName = friend?.Username ?? ""
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteFriendViewModel vm)
        {
            await _friendShipService.DeleteAsync(vm.FriendShipId);
            return RedirectToRoute(new { controller = "Friends", action = "Index" });
        }
    }
}

