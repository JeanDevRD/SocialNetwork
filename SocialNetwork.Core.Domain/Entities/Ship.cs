
namespace SocialNetwork.Core.Domain.Entities
{
    public class Ship : CommonEntity<int>
    {
        public Game? Game { get; set; }
        public User? Owner { get; set; }
        public required int Size { get; set; } 
    }
}
