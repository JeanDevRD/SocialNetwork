using SocialNetwork.Core.Application.DTOs.FriendRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.Interfaces
{
    public interface IFriendRequestService : IGenericServiceWithStatus<FriendRequestDto>
    {
        Task<List<string>> GetAllPendingByUserIdAsync(string userId);

    }
}
