using Microsoft.AspNetCore.Http;
using SocialNetwork.Core.Application.DTOs.Comment;
using SocialNetwork.Core.Application.DTOs.Reaction;
using SocialNetwork.Core.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.Post
{
    public class CreatePostViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Se requiere contenido")]
        [DataType(DataType.Text)]
        public required string Content { get; set; }
        public IFormFile? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public required DateTime Created { get; set; }
        public required string UserId { get; set; }
    }
}
