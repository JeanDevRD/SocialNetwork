using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.Comment
{
    public class CreateCommentViewModel
    {
        public required int Id { get; set; } = 0;
        [Required(ErrorMessage = "Se requiere contenido")]
        [DataType(DataType.Text)]
        public required string Content { get; set; }
        public DateTime? Created { get; set; }

        public string? UserId { get; set; }
        public required int PostId { get; set; }
        public int? ParentCommentId { get; set; }
    }
}
