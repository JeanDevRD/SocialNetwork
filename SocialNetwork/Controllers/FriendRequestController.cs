using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Core.Application.DTOs.FriendRequest;
using SocialNetwork.Core.Application.DTOs.Friendship;
using SocialNetwork.Core.Application.Interfaces;
using SocialNetwork.Core.Application.ViewModels.FriendRequest;
using SocialNetwork.Core.Domain.Enum;
using SocialNetwork.Infrastructure.Core.Application.Interfaces;
using SocialNetwork.Infrastructure.Identity.Entities;

namespace SocialNetwork.Controllers
{
    [Authorize]
    public class FriendRequestController : Controller
    {
        private readonly IFriendRequestService _friendRequestService;
        private readonly IFriendShipService _friendShipService;
        private readonly IAccountServiceWeb _accountService;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IMapper _mapper;

        public FriendRequestController(
            IFriendRequestService friendRequestService,
            IFriendShipService friendShipService,
            IAccountServiceWeb accountService,
            UserManager<UserEntity> userManager,
            IMapper mapper)
        {
            _friendRequestService = friendRequestService;
            _friendShipService = friendShipService;
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
            var allRequests = await _friendRequestService.GetAllAsync();

            var pendingRequestsEntities = allRequests
                .Where(r => r.ReceiverId.ToString() == currentUser.Id && r.Status == FriendRequestStatus.Pending)
                .ToList();

            var sentRequestsEntities = allRequests
                .Where(r => r.SenderId.ToString() == currentUser.Id).ToList();

            var pendingRequests = _mapper.Map<List<FriendRequestViewModel>>(pendingRequestsEntities);
            var sentRequests = _mapper.Map<List<FriendRequestViewModel>>(sentRequestsEntities);

            var currentUserFriends = await _friendShipService.GetAllFriendsIdAsync(currentUser.Id);

            foreach (var request in pendingRequests)
            {
                var senderFriends = await _friendShipService.GetAllFriendsIdAsync(request.SenderId.ToString());
                request.CommonFriends = currentUserFriends.Intersect(senderFriends).Count();
            }

           
            foreach (var request in sentRequests)
            {
                var receiverFriends = await _friendShipService.GetAllFriendsIdAsync(request.ReceiverId.ToString());
                request.CommonFriends = currentUserFriends.Intersect(receiverFriends).Count();
            }

            var viewModel = new FriendRequestIndexViewModel
            {
                PendingRequests = pendingRequests,
                SentRequests = sentRequests
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            var userSession = await _userManager.GetUserAsync(User);
            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }
            var currentUser = await _accountService.GetUserByUserName(userSession.UserName!);

         
            var allUsers = await _accountService.GetAllUser(true);

            
            var requests = await _friendRequestService.GetAllPendingByUserIdAsync(currentUser.Id);

           
            var friends = await _friendShipService.GetAllFriendsIdAsync(currentUser.Id);

           
            var availableUsers = allUsers
                .Where(u => u.Id != currentUser.Id)
                .Where(u => !friends.Contains(u.Id))
                .Where(u => !requests.Contains(u.Id))
                .Select(u => new AvailableUserViewModel
                {
                    Id = u.Id,
                    Username = u.Username,
                    Profile = u.Profile
                })
                .ToList();

            foreach (var user in availableUsers)
            {
                var userFriends = await _friendShipService.GetAllFriendsIdAsync(user.Id!);
                user.CommonFriends = friends.Intersect(userFriends).Count();
            }

            var viewModel = new CreateFriendRequestViewModel
            {
                AvailableUsers = availableUsers
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateFriendRequestViewModel vm)
        {
            var userSession = await _userManager.GetUserAsync(User);
            if (userSession == null)
                return RedirectToRoute(new { controller = "Login", action = "Index" });

            if (!ModelState.IsValid)
                return View(vm);

            var currentUser = await _accountService.GetUserByUserName(userSession.UserName!);

            var dto = new FriendRequestDto
            {
                Id = 0,
                SenderId = currentUser.Id,
                ReceiverId = vm.SelectedUserId,
                Status = FriendRequestStatus.Pending,
                RequestedAt = DateTime.Now
            };

            await _friendRequestService.AddAsync(dto);
            return RedirectToRoute(new { controller = "FriendRequest", action = "Index" });
        }

        public async Task<IActionResult> Accept(int id)
        {
            var request = await _friendRequestService.GetByIdAsync(id);
            if (request == null)
            {
                return RedirectToRoute(new { controller = "FriendRequest", action = "Index" });
            }

            var receiver = await _accountService.GetUserByUserName((await _userManager.FindByIdAsync(request.ReceiverId.ToString()))?.UserName!);

            var viewModel = new ConfirmActionViewModel
            {
                Id = id,
                Action = "Accept",
                Message = $"¿Está seguro que desea aceptar la solicitud de amistad del usuario {receiver?.Username}?"
            };

            return View("Confirm", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Accept(int id, string confirm)
        {
            await _friendRequestService.ChangeStatusAsync(id, FriendRequestStatus.Accepted);

            var request = await _friendRequestService.GetByIdAsync(id);
            var friendship = new FriendShipDto
            {
                Id = 0,
                UserId = request.SenderId,
                FriendId = request.ReceiverId,
                Created = DateTime.Now
            };

            await _friendShipService.AddAsync(friendship);
            return RedirectToRoute(new { controller = "FriendRequest", action = "Index" });
        }

        public async Task<IActionResult> Reject(int id)
        {
            var request = await _friendRequestService.GetByIdAsync(id);
            if (request == null)
                return RedirectToRoute(new { controller = "FriendRequest", action = "Index" });

            var receiver = await _accountService.GetUserByUserName((await _userManager.FindByIdAsync(request.ReceiverId.ToString()))?.UserName!);

            var viewModel = new ConfirmActionViewModel
            {
                Id = id,
                Action = "Reject",
                Message = $"¿Está seguro que desea rechazar la solicitud de amistad del usuario {receiver?.Username}?"
            };

            return View("Confirm", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id, string confirm)
        {
            await _friendRequestService.ChangeStatusAsync(id, FriendRequestStatus.Rejected);
            return RedirectToRoute(new { controller = "FriendRequest", action = "Index" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var request = await _friendRequestService.GetByIdAsync(id);
            if (request == null)
                return RedirectToRoute(new { controller = "FriendRequest", action = "Index" });

            var receiver = await _accountService.GetUserByUserName((await _userManager.FindByIdAsync(request.ReceiverId.ToString()))?.UserName!);

            var viewModel = new ConfirmActionViewModel
            {
                Id = id,
                Action = "Delete",
                Message = $"¿Está seguro que desea eliminar la solicitud de amistad para {receiver?.Username}?"
            };

            return View("Confirm", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, string confirm)
        {
            await _friendRequestService.DeleteAsync(id);
            return RedirectToRoute(new { controller = "FriendRequest", action = "Index" });
        }
    }
}

