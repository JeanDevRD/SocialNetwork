using SocialNetwork.Core.Application.DTOs.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.Interfaces
{
    public interface IGameService : IGenericService<GameDto>
    {
        Task<bool> StartGameAsync(int gameId);
        Task<bool> FinishGameAsync(int gameId, string winnerId);
        Task<bool> SurrenderGameAsync(int gameId, string surrenderingPlayerId);
    }
}
