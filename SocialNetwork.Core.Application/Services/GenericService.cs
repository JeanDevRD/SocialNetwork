﻿

using AutoMapper;
using SocialNetwork.Core.Application.Interfaces;
using SocialNetwork.Core.Domain.Interfaces;

namespace SocialNetwork.Core.Application.Services
{
    public class GenericService<EntityDto,Entity> : IGenericService<EntityDto> 
        where Entity : class
        where EntityDto : class
    {
        
        protected readonly IGenericRepository<Entity> _repo;
        protected readonly IMapper _mapper;


        public GenericService(IGenericRepository<Entity> repository, IMapper mapper)
        {
            _repo = repository;
            _mapper = mapper;
        }

        public async Task<EntityDto> AddAsync(EntityDto entityDto)
        {
            try
            {
                if (entityDto == null)
                {
                    return null!;
                }
                var entity = _mapper.Map<Entity>(entityDto);
                var result = await _repo.AddAsync(entity);
                return _mapper.Map<EntityDto>(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null)
                {
                    return false;
                }
                return await _repo.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        public async Task<List<EntityDto>> GetAllAsync()
        {
            try
            {
                var entities = await _repo.GetAllAsync();
                return _mapper.Map<List<EntityDto>>(entities);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<EntityDto> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null)
                {
                    return null!;
                }
                return _mapper.Map<EntityDto>(entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<EntityDto> UpdateAsync(int id, EntityDto entityDto)
        {
            try
            {
                if (entityDto == null)
                {
                    return null!;
                }
                var entity = _mapper.Map<Entity>(entityDto);
                var result = await _repo.UpdateAsync(id, entity);
                return _mapper.Map<EntityDto>(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }

        
    }

}
