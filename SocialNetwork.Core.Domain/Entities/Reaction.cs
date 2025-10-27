namespace SocialNetwork.Core.Domain.Entities
{
    public class Reaction : CommonEntity<int>
    {
        public required string Type { get; set; } 
        public required DateTime Created { get; set; }
        public required string UserId { get; set; }
        public required int PostId { get; set; }
        public Post? Post { get; set; }
    }
}
