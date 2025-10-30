using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Core.Application.DTOs.Attack;
using SocialNetwork.Core.Application.DTOs.Ship;
using SocialNetwork.Core.Application.Interfaces;
using SocialNetwork.Core.Application.ViewModels.Attack;
using SocialNetwork.Core.Application.ViewModels.Game;
using SocialNetwork.Core.Domain.Enum;
using SocialNetwork.Infrastructure.Identity.Entities;

namespace SocialNetwork.Controllers
{
    [Authorize]
    public class AttackController : Controller
    {
        private readonly IGameService _gameService;
        private readonly IShipService _shipService;
        private readonly IAttackService _attackService;
        private readonly IAccountServiceWeb _accountService;
        private readonly UserManager<UserEntity> _userManager;

        public AttackController(IGameService gameService, IShipService shipService, IAttackService attackService,
            IAccountServiceWeb accountService, UserManager<UserEntity> userManager)
        {
            _gameService = gameService;
            _shipService = shipService;
            _attackService = attackService;
            _accountService = accountService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Attack(int id)
        {
            var userSession = await _userManager.GetUserAsync(User);
            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }

            var user = await _accountService.GetUserByUserName(userSession.UserName!);
            var game = await _gameService.GetByIdAsync(id);

            if (game == null)
            {
                ViewBag.Error = "Partida no encontrada";
                return RedirectToRoute(new { controller = "Game", action = "Index" });
            }

            if (game.Player1Id != user.Id && game.Player2Id != user.Id)
            {
                ViewBag.Error = "No tienes acceso a esta partida";
                return RedirectToRoute(new { controller = "Game", action = "Index" });
            }

            if (game.Status != GameStatus.Active)
            {
                return RedirectToRoute(new { controller = "Game", action = "SelectShip", id = id });
            }

            var opponentId = game.Player1Id == user.Id ? game.Player2Id : game.Player1Id;
            var opponent = await _userManager.FindByIdAsync(opponentId);

         
            var allAttacks = await _attackService.GetAllAsync();
            var userAttacks = allAttacks.Where(a => a.GameId == id && a.AttackerId == user.Id).ToList();

            var viewModel = new AttackBoardViewModel
            {
                GameId = id,
                IsMyTurn = game.CurrentTurnPlayerId == user.Id,
                OpponentUsername = opponent?.UserName ?? "Oponente"
            };

          
            foreach (var attack in userAttacks)
            {
                if (attack.Row >= 0 && attack.Row < 12 && attack.Column >= 0 && attack.Column < 12)
                {
                    viewModel.Board[attack.Row, attack.Column].Status = attack.Hit? AttackStatus.Hit : AttackStatus.Miss;
                }
            }

            return View(viewModel);
        }

        private List<(int Row, int Column)> GetShipPositions(int startRow, int startColumn, int size, string direction)
        {
            var positions = new List<(int, int)>();

            for (int i = 0; i < size; i++)
            {
                switch (direction)
                {
                    case "Up":
                        positions.Add((startRow - i, startColumn));
                        break;
                    case "Down":
                        positions.Add((startRow + i, startColumn));
                        break;
                    case "Left":
                        positions.Add((startRow, startColumn - i));
                        break;
                    case "Right":
                        positions.Add((startRow, startColumn + i));
                        break;
                }
            }

            return positions;
        }

        [HttpPost]
        public async Task<IActionResult> PerformAttack(int gameId, int row, int column)
        {
            var userSession = await _userManager.GetUserAsync(User);
            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }

            var user = await _accountService.GetUserByUserName(userSession.UserName!);
            var game = await _gameService.GetByIdAsync(gameId);

            if (game == null)
            {
                TempData["Error"] = "Partida no encontrada";
                return RedirectToRoute(new { controller = "Game", action = "Index" });
            }

            if (game.CurrentTurnPlayerId != user.Id)
            {
                TempData["Error"] = "No es tu turno";
                return RedirectToRoute(new { controller = "Attack", action = "Attack", id = gameId });
            }

 
            var allAttacks = await _attackService.GetAllAsync();
            var existingAttack = allAttacks.FirstOrDefault(a => a.GameId == gameId && a.AttackerId == user.Id 
            && a.Row == row && a.Column == column );

            if (existingAttack != null)
            {
                TempData["Error"] = "Ya atacaste esta celda";
                return RedirectToRoute(new { controller = "Attack", action = "Attack", id = gameId });
            }

            var opponentId = game.Player1Id == user.Id ? game.Player2Id : game.Player1Id;

            var allShips = await _shipService.GetAllAsync();
            var opponentShips = allShips.Where(s => s.GameId == gameId && s.OwnerId == opponentId).ToList();

            bool isHit = false;
            ShipDto? hitShip = null;

            foreach (var ship in opponentShips)
            {
                var shipPositions = GetShipPositions(ship.StartRow, ship.StartColumn, ship.Size, ship.Direction);

                if (shipPositions.Any(p => p.Row == row && p.Column == column))
                {
                    isHit = true;
                    hitShip = ship;
                    break;
                }
            }

            try
            {
                var attackDto = new AttackDto
                {
                    Id = 0,
                    GameId = gameId,
                    AttackerId = user.Id,
                    Row = row,
                    Column = column,
                    Hit = isHit,
                    Attacked = DateTime.Now
                };

                await _attackService.AddAsync(attackDto);

                if (isHit && hitShip != null)
                {
                    var shipPositions = GetShipPositions(hitShip.StartRow, hitShip.StartColumn, hitShip.Size, hitShip.Direction);

                    allAttacks = await _attackService.GetAllAsync();

                    var attacksOnThisShip = allAttacks.Where(a => a.GameId == gameId && a.AttackerId == user.Id && a.Hit)
                        .Count(a => shipPositions.Any(p => p.Row == a.Row && p.Column == a.Column));

                    if (attacksOnThisShip >= hitShip.Size)
                    {
                        hitShip.IsSunk = true;
                        await _shipService.UpdateAsync(hitShip.Id, hitShip);

                        var updatedShips = await _shipService.GetAllAsync();
                        var opponentShipsUpdated = updatedShips.Where(s => s.GameId == gameId && s.OwnerId == opponentId).ToList();

                        if (opponentShipsUpdated.All(s => s.IsSunk))
                        {
                            await _gameService.FinishGameAsync(gameId, user.Id);
                            TempData["Success"] = "Has ganado la partida!";
                            return RedirectToRoute(new { controller = "Game", action = "Result", id = gameId });
                        }
                    }
                }
                game.CurrentTurnPlayerId = opponentId;
                await _gameService.UpdateAsync(gameId, game);

                return RedirectToRoute(new { controller = "Attack", action = "Attack", id = gameId });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                TempData["Error"] = "Error al procesar el ataque";
                return RedirectToRoute(new { controller = "Attack", action = "Attack", id = gameId });
            }
        }

        public async Task<IActionResult> ViewMyBoard(int id)
        {
            var userSession = await _userManager.GetUserAsync(User);
            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }

            var user = await _accountService.GetUserByUserName(userSession.UserName!);
            var game = await _gameService.GetByIdAsync(id);

            if (game == null)
            {
                ViewBag.Error = "Partida no encontrada";
                return RedirectToRoute(new { controller = "Game", action = "Index" });
            }

            var allShips = await _shipService.GetAllAsync();
            var userShips = allShips.Where(s => s.GameId == id && s.OwnerId == user.Id).ToList();

            var viewModel = new BoardViewModel { GameId = id, ShipSize = 0 };

            foreach (var ship in userShips)
            {
                var positions = GetShipPositions(ship.StartRow, ship.StartColumn, ship.Size, ship.Direction);
                foreach (var pos in positions)
                {
                    if (pos.Row >= 0 && pos.Row < 12 && pos.Column >= 0 && pos.Column < 12)
                    {
                        viewModel.Board[pos.Row, pos.Column].IsOccupied = true;
                    }
                }
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Surrender(int id)
        {
            var userSession = await _userManager.GetUserAsync(User);
            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }

            var user = await _accountService.GetUserByUserName(userSession.UserName!);
            var game = await _gameService.GetByIdAsync(id);

            if (game == null)
            {
                ViewBag.Error = "Partida no encontrada";
                return RedirectToRoute(new { controller = "Game", action = "Index" });
            }

            var opponentId = game.Player1Id == user.Id ? game.Player2Id : game.Player1Id;
            var opponent = await _userManager.FindByIdAsync(opponentId);

            var viewModel = new SurrenderViewModel
            {
                GameId = id,
                OpponentUsername = opponent?.UserName ?? "Oponente"
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmSurrender(int gameId)
        {
            var userSession = await _userManager.GetUserAsync(User);
            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }

            var user = await _accountService.GetUserByUserName(userSession.UserName!);
            var game = await _gameService.GetByIdAsync(gameId);

            if (game == null)
            {
                TempData["Error"] = "Partida no encontrada";
                return RedirectToRoute(new { controller = "Game", action = "Index" });
            }

            try
            {

                await _gameService.SurrenderGameAsync(gameId, user.Id);

                TempData["Info"] = "Te has rendido. El oponente gano la partida.";
                return RedirectToRoute(new { controller = "Game", action = "Index" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                TempData["Error"] = "Error al rendirse";
                return RedirectToRoute(new { controller = "Attack", action = "Attack", id = gameId });
            }
        }

       
    }
}
