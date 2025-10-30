using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Core.Application.DTOs.Game;
using SocialNetwork.Core.Application.DTOs.Ship;
using SocialNetwork.Core.Application.Interfaces;
using SocialNetwork.Core.Application.ViewModels.Game;
using SocialNetwork.Core.Application.ViewModels.Ship;
using SocialNetwork.Core.Domain.Enum;
using SocialNetwork.Infrastructure.Identity.Entities;

namespace SocialNetwork.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly IGameService _gameService;
        private readonly IShipService _shipService;
        private readonly IAccountServiceWeb _accountService;
        private readonly IFriendShipService _friendShipService;
        private readonly UserManager<UserEntity> _userManager;

        public GameController(IGameService gameService, IShipService shipService, IAccountServiceWeb accountService,
            IFriendShipService friendShipService, UserManager<UserEntity> userManager)
        {
            _gameService = gameService;
            _shipService = shipService;
            _accountService = accountService;
            _friendShipService = friendShipService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userSession = await _userManager.GetUserAsync(User);
            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }

            var user = await _accountService.GetUserByUserName(userSession.UserName!);
            var allGames = await _gameService.GetAllAsync();

            var userGames = allGames.Where(g => g.Player1Id == user.Id
            || g.Player2Id == user.Id).ToList();

            var viewModel = new GameViewModel();

            var activeGames = userGames.Where(g => g.Status == GameStatus.Waiting
            || g.Status == GameStatus.Active).OrderByDescending(g => g.Started).ToList();

            foreach (var game in activeGames)
            {
                var opponentId = game.Player1Id == user.Id ? game.Player2Id : game.Player1Id;
                var opponent = await _userManager.FindByIdAsync(opponentId);

                var elapsed = DateTime.Now - game.Started;
                var elapsedHours = (int)elapsed.TotalHours;

                viewModel.ActiveGames.Add(new ActiveGameViewModel
                {
                    GameId = game.Id,
                    OpponentUsername = opponent?.UserName ?? "Usuario",
                    OpponentProfile = opponent?.Profile ?? "Images/default_profile.png",
                    Started = game.Started,
                    ElapsedTime = $"{elapsedHours} horas",
                    Status = game.Status
                });
            }

            var finishedGames = userGames.Where(g => g.Status == GameStatus.Finished)
                .OrderByDescending(g => g.Ended ?? g.Started).ToList();

            foreach (var game in finishedGames)
            {
                var opponentId = game.Player1Id == user.Id ? game.Player2Id : game.Player1Id;
                var opponent = await _userManager.FindByIdAsync(opponentId);

                var duration = (game.Ended ?? DateTime.Now) - game.Started;
                var durationHours = (int)duration.TotalHours;

                var won = game.WinnerId == user.Id;
                var winnerUser = await _userManager.FindByIdAsync(game.WinnerId ?? "");

                viewModel.HistoryGames.Add(new HistoryGameViewModel
                {
                    GameId = game.Id,
                    OpponentUsername = opponent?.UserName ?? "Usuario",
                    OpponentProfile = opponent?.Profile ?? "Images/default_profile.png",
                    Started = game.Started,
                    Ended = game.Ended ?? DateTime.Now,
                    Duration = $"{durationHours} horas",
                    Won = won,
                    WinnerUsername = game.WinnerId == user.Id ? "Yo" : (winnerUser?.UserName ?? "Oponente")
                });
            }

            viewModel.Stats.TotalGames = finishedGames.Count;
            viewModel.Stats.GamesWon = finishedGames.Count(g => g.WinnerId == user.Id);
            viewModel.Stats.GamesLost = finishedGames.Count(g => g.WinnerId != user.Id);

            return View(viewModel);
        }
    
        public async Task<IActionResult> Create()
        {
            var userSession = await _userManager.GetUserAsync(User);
            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }

            var user = await _accountService.GetUserByUserName(userSession.UserName!);


            var friendIds = await _friendShipService.GetAllFriendsIdAsync(user.Id);

            var allGames = await _gameService.GetAllAsync();
            var activeGames = allGames.Where(g => (g.Player1Id == user.Id || g.Player2Id == user.Id)
                       && (g.Status == GameStatus.Waiting || g.Status == GameStatus.Active)).ToList();

            var friendsWithActiveGames = activeGames.Select(g => g.Player1Id == user.Id ? g.Player2Id 
            : g.Player1Id).ToHashSet();

            var viewModel = new CreateGameViewModel();

            foreach (var friendId in friendIds)
            {
                if (!friendsWithActiveGames.Contains(friendId))
                {
                    var friend = await _userManager.FindByIdAsync(friendId);
                    if (friend != null)
                    {
                        viewModel.AvailableFriends.Add(new AvailableFriendViewModel
                        {
                            Id = friend.Id,
                            Username = friend.UserName ?? "Usuario",
                            Profile = friend.Profile
                        });
                    }
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateGameViewModel vm)
        {
            var userSession = await _userManager.GetUserAsync(User);
            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Debe seleccionar un amigo para jugar";
                return View(vm);
            }

            var user = await _accountService.GetUserByUserName(userSession.UserName!);

            var allGames = await _gameService.GetAllAsync();
            var existingGame = allGames.FirstOrDefault(g =>
                ((g.Player1Id == user.Id && g.Player2Id == vm.SelectedFriendId) ||
                 (g.Player1Id == vm.SelectedFriendId && g.Player2Id == user.Id)) &&
                (g.Status == GameStatus.Waiting || g.Status == GameStatus.Active));

            if (existingGame != null)
            {
                ViewBag.Error = "Ya existe una partida activa con este usuario";
                return View(vm);
            }

            try
            {
                var gameDto = new GameDto
                {
                    Id = 0,
                    Player1Id = user.Id,
                    Player2Id = vm.SelectedFriendId!,
                    Started = DateTime.Now,
                    Ended = null,
                    Status = GameStatus.Waiting,
                    WinnerId = null,
                    CurrentTurnPlayerId = user.Id 
                };

                var createdGame = await _gameService.AddAsync(gameDto);

                return RedirectToRoute(new { controller = "Game", action = "SelectShip", id = createdGame.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                ViewBag.Error = "Error al crear la partida. Intente nuevamente.";
                return View(vm);
            }
        }

        public async Task<IActionResult> SelectShip(int id)
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

            var allShips = await _shipService.GetAllAsync();
            var userShips = allShips.Where(s => s.GameId == id && s.OwnerId == user.Id).ToList();

            var requiredShips = new List<int> { 5, 4, 3, 3, 2 };

     
            var placedSizes = userShips.Select(s => s.Size).ToList();
            var availableShips = new List<int>();

            foreach (var size in requiredShips)
            {
                if (!placedSizes.Contains(size))
                {
                    availableShips.Add(size);
                }
                else
                {
                    placedSizes.Remove(size);
                }
            }
            if (availableShips.Count == 0)
            {
                var opponentId = game.Player1Id == user.Id ? game.Player2Id : game.Player1Id;

                var opponentShips = allShips.Where(s => s.GameId == id && s.OwnerId == opponentId).ToList();

                if (opponentShips.Count >= 5)
                {
                    await _gameService.StartGameAsync(id);
                    return RedirectToRoute(new { controller = "Attack", action = "Attack", id = id });
                }
                else
                {
                    return RedirectToRoute(new { controller = "Game", action = "WaitingOpponent", id = id });
                }
            }

            var viewModel = new SelectShipViewModel
            {
                GameId = id,
                AvailableShips = availableShips.Select(size => new ShipOptionViewModel
                {
                    Size = size,
                    Description = $"Barco de {size} posiciones"
                }).ToList()
            };

            return View(viewModel);
        }
        public async Task<IActionResult> SelectPosition(int gameId, int shipSize)
        {
            var userSession = await _userManager.GetUserAsync(User);
            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }

            var user = await _accountService.GetUserByUserName(userSession.UserName!);
            var game = await _gameService.GetByIdAsync(gameId);

            if (game == null || (game.Player1Id != user.Id && game.Player2Id != user.Id))
            {
                ViewBag.Error = "No tienes acceso a esta partida";
                return RedirectToRoute(new { controller = "Game", action = "Index" });
            }

            var allShips = await _shipService.GetAllAsync();
            var userShips = allShips.Where(s => s.GameId == gameId && s.OwnerId == user.Id).ToList();

            var viewModel = new BoardViewModel
            {
                GameId = gameId,
                ShipSize = shipSize
            };

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
        public IActionResult SelectDirection(int gameId, int shipSize, int row, int column)
        {
            var viewModel = new SelectDirectionViewModel
            {
                GameId = gameId,
                ShipSize = shipSize,
                Row = row,
                Column = column
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceShip(PlaceShipViewModel vm)
        {
            var userSession = await _userManager.GetUserAsync(User);
            if (userSession == null)
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }

            if (!ModelState.IsValid)
            {
                return RedirectToRoute(new {controller = "Game", action = "SelectDirection", gameId = vm.GameId, shipSize = vm.ShipSize,
                    row = vm.StartRow, column = vm.StartColumn });
            }

            var user = await _accountService.GetUserByUserName(userSession.UserName!);
            var game = await _gameService.GetByIdAsync(vm.GameId);

            if (game == null)
            {
                ViewBag.Error = "Partida no encontrada";
                return RedirectToRoute(new { controller = "Game", action = "Index" });
            }

            var newShipPositions = GetShipPositions(vm.StartRow, vm.StartColumn, vm.ShipSize, vm.Direction!);

            foreach (var pos in newShipPositions)
            {
                if (pos.Row < 0 || pos.Row >= 12 || pos.Column < 0 || pos.Column >= 12)
                {
                    TempData["Error"] = "El barco se sale del tablero. Cambia la celda o dirección.";

                    return RedirectToRoute(new {controller = "Game", action = "SelectDirection", gameId = vm.GameId,
                        shipSize = vm.ShipSize, row = vm.StartRow, column = vm.StartColumn });
                }
            }

            var allShips = await _shipService.GetAllAsync();
            var userShips = allShips.Where(s => s.GameId == vm.GameId && s.OwnerId == user.Id).ToList();

            foreach (var existingShip in userShips)
            {
                var existingPositions = GetShipPositions( existingShip.StartRow, existingShip.StartColumn,
                    existingShip.Size, existingShip.Direction );

                foreach (var newPos in newShipPositions)
                {
                    if (existingPositions.Any(ep => ep.Row == newPos.Row && ep.Column == newPos.Column))
                    {
                        TempData["Error"] = "El barco colisiona con otro ya posicionado. Cambia la celda o dirección.";

                        return RedirectToRoute(new { controller = "Game", action = "SelectDirection", gameId = vm.GameId, shipSize = vm.ShipSize, 
                            row = vm.StartRow, column = vm.StartColumn });
                    }
                }
            }
            try
            {
                var shipDto = new ShipDto
                {
                    Id = 0,
                    GameId = vm.GameId,
                    OwnerId = user.Id,
                    Size = vm.ShipSize,
                    StartRow = vm.StartRow,
                    StartColumn = vm.StartColumn,
                    Direction = vm.Direction!,
                    IsSunk = false
                };

                await _shipService.AddAsync(shipDto);
                return RedirectToRoute(new { controller = "Game", action = "SelectShip", id = vm.GameId });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                TempData["Error"] = "Error al posicionar el barco";

                return RedirectToRoute(new { controller = "Game", action = "SelectDirection", gameId = vm.GameId,
                    shipSize = vm.ShipSize, row = vm.StartRow, column = vm.StartColumn });
            }

        }
        public async Task<IActionResult> WaitingOpponent(int id)
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
    }
}