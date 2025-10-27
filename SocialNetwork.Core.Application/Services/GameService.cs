using AutoMapper;
using SocialNetwork.Core.Application.DTOs.Game;
using SocialNetwork.Core.Application.Interfaces;
using SocialNetwork.Core.Domain.Entities;
using SocialNetwork.Core.Domain.Enum;
using SocialNetwork.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.Services
{
    public class GameService : GenericService<GameDto, Game>, IGameService
    {
        public GameService(IGenericRepository<Game> repository, IMapper mapper)
            : base(repository, mapper)
        {
        }

        public async Task<bool> StartGameAsync(int gameId)
        {
            try
            {
                var game = await _repo.GetByIdAsync(gameId);
                if (game == null) return false;

                if (game.Status != GameStatus.Waiting)
                    return false;

                game.Status = GameStatus.Active;
                await _repo.UpdateAsync(gameId, game);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> FinishGameAsync(int gameId, string winnerId)
        {
            try
            {
                var game = await _repo.GetByIdAsync(gameId);
                if (game == null) return false;

                if (game.Status == GameStatus.Finished)
                    return false;

                game.Status = GameStatus.Finished;
                game.WinnerId = winnerId;
                game.Ended = DateTime.Now;

                await _repo.UpdateAsync(gameId, game);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> SurrenderGameAsync(int gameId, string surrenderingPlayerId)
        {
            try
            {
                var game = await _repo.GetByIdAsync(gameId);
                if (game == null) return false;

                string winnerId = game.Player1Id == surrenderingPlayerId
                    ? game.Player2Id
                    : game.Player1Id;

                return await FinishGameAsync(gameId, winnerId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
    
}
