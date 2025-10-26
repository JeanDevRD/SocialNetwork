using AutoMapper;
using SocialNetwork.Core.Application.DTOs.Friendship;
using SocialNetwork.Core.Application.Interfaces;
using SocialNetwork.Core.Domain.Entities;
using SocialNetwork.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.Services
{
    public class FriendshipService : GenericService<FriendShipDto, FriendShip>, IFriendShipService
    {
        public FriendshipService(IGenericRepository<FriendShip> repository, IMapper mapper) 
            : base(repository, mapper)
        {
        }
    }
}
