using AutoMapper;
using SocialNetwork.Core.Application.DTOs.FriendRequest;
using SocialNetwork.Core.Application.Interfaces;
using SocialNetwork.Core.Domain.Entities;
using SocialNetwork.Core.Domain.Enum;
using SocialNetwork.Core.Domain.Interfaces;

namespace SocialNetwork.Core.Application.Services
{
    public class FriendRequestService : GenericService<FriendRequestDto, FriendRequest>, IFriendRequestService
    {
        public FriendRequestService(IGenericRepository<FriendRequest> repository, IMapper mapper)
            : base(repository, mapper)
        {
        }

        public async Task<bool> ChangeStatusAsync(int id, string newStatus)
        {
            try
            {
                var friendRequest = await _repo.GetByIdAsync(id);
                if (friendRequest == null || friendRequest.Status != FriendRequestStatus.Pending)
                {
                    return false;
                }

                
                if (newStatus != FriendRequestStatus.Accepted &&
                    newStatus != FriendRequestStatus.Rejected)
                {
                    return false;
                }

                friendRequest.Status = newStatus;
                await _repo.UpdateAsync(id, friendRequest);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        
        public async Task<List<string>> GetAllPendingByUserIdAsync(string userId)
        {
            var allRequests = await GetAllAsync();

            return allRequests
                .Where(r => r.Status == FriendRequestStatus.Pending && (r.SenderId == userId || r.ReceiverId == userId))
                .Select(r => r.SenderId == userId ? r.ReceiverId : r.SenderId)
                .Distinct()
                .ToList();
        }
    }
}
