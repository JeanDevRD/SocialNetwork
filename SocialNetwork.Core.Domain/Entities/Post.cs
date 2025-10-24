
namespace SocialNetwork.Core.Domain.Entities
{
    public class Post : CommonEntity<int>
    {
        public required string Content { get; set; }
        public string? ImageUrl { get; set; }   
        public string? VideoUrl { get; set; }    
        public required DateTime Created { get; set; }

        public required int UserId { get; set; }

        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Reaction>? Reactions { get; set; }
    }
}
