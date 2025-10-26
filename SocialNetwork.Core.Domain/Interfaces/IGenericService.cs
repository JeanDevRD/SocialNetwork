using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Domain.Interfaces
{
    public interface IGenericService<TDto>
    {
        Task<List<TDto>> GetAllAsync();
        Task<TDto> GetByIdAsync(int id);
        Task<TDto> AddAsync(TDto dto);
        Task<TDto> UpdateAsync(int id, TDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
