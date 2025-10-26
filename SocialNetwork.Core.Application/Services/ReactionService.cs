using AutoMapper;
using SocialNetwork.Core.Application.DTOs.Reaction;
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
    public class ReactionService : GenericService<ReactionDto, Reaction>, IReactionService
    {
        public ReactionService(IGenericRepository<Reaction> repository, IMapper mapper) 
            : base(repository, mapper)
        {
        }

    }
}
