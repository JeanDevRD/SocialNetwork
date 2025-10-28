using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Core.Application.Interfaces;
using SocialNetwork.Core.Application.ViewModels.Home;
using SocialNetwork.Infrastructure.Identity.Entities;


namespace SocialNetwork.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        private readonly IReactionService _reactionService;
        private readonly IAccountServiceWeb _accountService;
        private readonly UserManager<UserEntity> _userManager;

        public HomeController(IPostService postService, ICommentService commentService,IReactionService reactionService,
            IAccountServiceWeb accountService, UserManager<UserEntity> userManager)
        {
            _postService = postService;
            _commentService = commentService;
            _reactionService = reactionService;
            _accountService = accountService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userSession = await _userManager.GetUserAsync(User);
            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }

            var currentUser = await _accountService.GetUserByUserName(userSession.UserName!);
            var allPosts = await _postService.GetAllAsync();
            var allComments = await _commentService.GetAllAsync();
            var allReactions = await _reactionService.GetAllAsync();

            var userPosts = allPosts.Where(p => p.UserId == currentUser.Id)
                .OrderByDescending(p => p.Created)
                .ToList();

            var viewModel = new HomeViewModel();

            ViewBag.CurrentUserId = currentUser.Id;

            foreach (var post in userPosts)
            {
                var author = await _userManager.FindByIdAsync(post.UserId);

                var postComments = allComments.Where(c => c.PostId == post.Id && c.ParentCommentId == null)
                    .OrderBy(c => c.Created).ToList();

                var postReactions = allReactions.Where(r => r.PostId == post.Id).ToList();

                var postDetail = new PostDetailViewModel
                {
                    Id = post.Id,
                    Content = post.Content,
                    ImageUrl = post.ImageUrl,
                    VideoUrl = post.VideoUrl,
                    Created = post.Created,
                    AuthorId = post.UserId,
                    AuthorName = author?.UserName ?? "Usuario",
                    AuthorProfile = author?.Profile ?? "Images/default_profile.png",
                    Comments = new List<CommentDetailViewModel>(),
                    Reactions = postReactions.Select(r => new ReactionDetailViewModel
                    {
                        Id = r.Id,
                        Type = r.Type,
                        UserId = r.UserId
                    }).ToList(),
                    CurrentUserLiked = postReactions.Any(r => r.UserId == currentUser.Id && r.Type == "Like"),  
                    CurrentUserDisliked = postReactions.Any(r => r.UserId == currentUser.Id && r.Type == "Dislike"),  
                    LikesCount = postReactions.Count(r => r.Type == "Like"),
                    DislikesCount = postReactions.Count(r => r.Type == "Dislike")
                };

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

                    var replies = allComments
                        .Where(c => c.ParentCommentId == comment.Id)
                        .OrderBy(c => c.Created)
                        .ToList();

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

                    postDetail.Comments.Add(commentDetail);
                }

                viewModel.Posts.Add(postDetail);
            }

            return View(viewModel);
        }
    }
}