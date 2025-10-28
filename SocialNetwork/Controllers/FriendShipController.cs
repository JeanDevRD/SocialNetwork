using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Core.Application.Interfaces;
using SocialNetwork.Core.Application.Services;
using SocialNetwork.Core.Application.ViewModels.FriendShip;
using SocialNetwork.Core.Application.ViewModels.Home;
using SocialNetwork.Infrastructure.Identity.Entities;

namespace SocialNetwork.Controllers
{
    [Authorize]
    public class FriendShipController : Controller
    {
        private readonly IFriendShipService _friendShipService;
        private readonly IPostService _postService;
        private readonly IAccountServiceWeb _accountService;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IMapper _mapper;
        private readonly ICommentService _commentService;
        private readonly IReactionService _reactionService;

        public FriendShipController( IFriendShipService friendShipService, IPostService postService, IAccountServiceWeb accountService,
            UserManager<UserEntity> userManager, IMapper mapper, ICommentService comment,IReactionService reaction)
        {
            _friendShipService = friendShipService;
            _postService = postService;
            _accountService = accountService;
            _userManager = userManager;
            _mapper = mapper;
            _commentService = comment;
            _reactionService = reaction;
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

            var currentUser = await _accountService.GetUserByUserName(userSession.UserName!);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return RedirectToRoute(new { controller = "FriendShip", action = "Index" });

            var allPosts = await _postService.GetAllAsync();
            var allComments = await _commentService.GetAllAsync();
            var allReactions = await _reactionService.GetAllAsync();

            var userPosts = allPosts.Where(p => p.UserId == userId)
                .OrderByDescending(p => p.Created)
                .ToList();

            var postDetails = new List<FriendPostViewModel>();

            foreach (var post in userPosts)
            {
                var postComments = allComments.Where(c => c.PostId == post.Id && c.ParentCommentId == null)
                    .OrderBy(c => c.Created).ToList();
                var postReactions = allReactions.Where(r => r.PostId == post.Id).ToList();

                var friendPost = _mapper.Map<FriendPostViewModel>(post);

                friendPost.CommentsCount = allComments.Count(c => c.PostId == post.Id);
                friendPost.LikesCount = postReactions.Count(r => r.Type == "Like");
                friendPost.DislikesCount = postReactions.Count(r => r.Type == "Dislike");

                friendPost.Comments = new List<CommentDetailViewModel>();

                foreach (var comment in postComments)
                {
                    var commentAuthor = await _userManager.FindByIdAsync(comment.UserId);
                    var commentDetail = new CommentDetailViewModel
                    {
                        Id = comment.Id,
                        Content = comment.Content,
                        Created = comment.Created,
                        AuthorId = comment.UserId,
                        AuthorName = commentAuthor?.UserName ?? "Usuario",
                        AuthorProfile = commentAuthor?.Profile ?? "Images/default_profile.png",
                        ParentCommentId = comment.ParentCommentId,
                        Replies = new List<CommentDetailViewModel>()
                    };

                    var replies = allComments.Where(c => c.ParentCommentId == comment.Id).OrderBy(c => c.Created).ToList();

                    foreach (var reply in replies)
                    {
                        var replyAuthor = await _userManager.FindByIdAsync(reply.UserId);
                        commentDetail.Replies.Add(new CommentDetailViewModel
                        {
                            Id = reply.Id,
                            Content = reply.Content,
                            Created = reply.Created,
                            AuthorId = reply.UserId,
                            AuthorName = replyAuthor?.UserName ?? "Usuario",
                            AuthorProfile = replyAuthor?.Profile ?? "Images/default_profile.png",
                            ParentCommentId = reply.ParentCommentId,
                            Replies = new List<CommentDetailViewModel>()
                        });
                    }

                    friendPost.Comments.Add(commentDetail);
                }

                postDetails.Add(friendPost);
            }

            ViewBag.CurrentUserId = currentUser.Id;

            var viewModel = new UserPostsViewModel
            {
                UserId = userId,
                UserName = user.UserName ?? "",
                UserProfile = user.Profile,
                Posts = postDetails
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

