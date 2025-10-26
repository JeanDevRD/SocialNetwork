using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.Interfaces
{

    public interface IGenericServiceWithStatus<TDto> : IGenericService<TDto>
    {
      Task<bool> ChangeStatusAsync(int id, string newStatus);
    }
}
