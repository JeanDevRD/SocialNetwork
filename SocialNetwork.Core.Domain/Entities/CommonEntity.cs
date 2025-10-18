
namespace SocialNetwork.Core.Domain.Entities
{
    public class CommonEntity<TKey>
    {
        public required TKey Id { get; set; }
    }
}
