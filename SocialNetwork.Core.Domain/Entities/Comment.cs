
namespace SocialNetwork.Core.Domain.Entities
{
    public class Comment : CommonEntity<int>
    {
        public  required string Content { get; set; }
        public required DateTime Created { get; set; }

        public required int UserId { get; set; }
        public required int PostId { get; set; }
        public int? ParentCommentId { get; set; }

        public User? Author { get; set; }
        public Post? Post { get; set; }
        public Comment? ParentComment { get; set; }
        public ICollection<Comment>? Replies { get; set; }
    }
}
