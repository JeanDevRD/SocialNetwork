using AutoMapper;
using SocialNetwork.Core.Application.DTOs.Attack;
using SocialNetwork.Core.Application.Interfaces;
using SocialNetwork.Core.Domain.Entities;
using SocialNetwork.Core.Domain.Interfaces;

namespace SocialNetwork.Core.Application.Services
{
    public class AttackService : GenericService<AttackDto, Attack>, IAttackService
    {
        public AttackService(IGenericRepository<Attack> repository, IMapper mapper) 
            : base(repository, mapper)
        {
        }
    }
}
