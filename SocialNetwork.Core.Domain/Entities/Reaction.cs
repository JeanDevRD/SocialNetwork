namespace SocialNetwork.Core.Domain.Entities
{
    public class Reaction : CommonEntity<int>
    {
        public required string Content { get; set; }
        public required DateTime Created { get; set; }

        public User? Author { get; set; }
        public Post? Post { get; set; }

        public Comment? ParentComment { get; set; }
        public ICollection<Comment>? Replies { get; set; }
    }
}
