using Microsoft.AspNetCore.Http;
using SocialNetwork.Core.Application.DTOs.Comment;
using SocialNetwork.Core.Application.DTOs.Reaction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.Post
{
    public class EditPostViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Se requiere contenido")]
        [DataType(DataType.Text)]
        public required string Content { get; set; }
        public IFormFile? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public required DateTime Created { get; set; }
    }
}
