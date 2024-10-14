using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using CheckersApi.Models;

namespace CheckersApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly ILogger<GameController> _logger;

        private static Game _game = new Game();
        private static int Player1Pieces = 12;
        private static int Player2Pieces = 12;

        public GameController(ILogger<GameController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<Game> Get()
        {
            return _game;
        }

        [HttpPost("move")]
        public IActionResult Move([FromBody] MoveRequest request)
        {
            var checker = _game.Board[request.FromRow][request.FromCol];


            _logger.LogInformation("current turn: " + _game.CurrentTurn);
            _logger.LogInformation("current turn on board: " + checker.player);

            if (checker == null || checker.player != _game.CurrentTurn)
                return BadRequest("Invalid move.");

            if (IsValidMove(request, out var captured))
            {
                // Move the checker
                _game.Board[request.ToRow][request.ToCol] = checker;
                _game.Board[request.FromRow][request.FromCol] = null;

                // if we captured the opponents piece = reset the middle location
                //ex: from = (2,7) to = (4,5) -> (2+4)/2, (7+5)/2 = middle = (3,6)
                if (captured)
                {

                    _game.Board[(request.FromRow + request.ToRow) / 2][(request.FromCol + request.ToCol) / 2] = null;
                    if (_game.CurrentTurn == "player1") Player2Pieces--;
                    else Player1Pieces--;

                }

                // Check for game end conditions
                if (CheckGameEnd())
                {
                    return Ok(new { Game = _game, Message = $"{_game.CurrentTurn} wins!" });
                }

                _game.CurrentTurn = _game.CurrentTurn == "player1" ? "player2" : "player1";
                return Ok(_game);
            }

            return BadRequest("Invalid move.");
        }

        private bool IsValidMove(MoveRequest request, out bool captured)
        {
            captured = false;
            _logger.LogInformation("from row: " + request.FromRow + ", from col: " + request.FromCol);
            _logger.LogInformation("to row: " + request.ToRow + ", to col: " + request.ToCol);

            // player1 moves "down" (increasing rows), player2 moves "up" (decreasing rows)
            int direction = _game.CurrentTurn == "player1" ? 1 : -1;

            // checking both diagonals
            if (request.ToRow == request.FromRow + direction && request.ToCol == request.FromCol + 1 &&
                _game.Board[request.ToRow][request.ToCol] == null) return true;

            if (request.ToRow == request.FromRow + direction && request.ToCol == request.FromCol - 1 &&
                _game.Board[request.ToRow][request.ToCol] == null) return true;

            // check if can capture, we just need to add 2 here instead of 1
            // in each "jump", we check the following:
            //1.if theres a piece in the middle
            //2.if the piece in the middle isn't the current player's piece
            //3.if our desired location is available

            if (request.ToRow == request.FromRow + 2 * direction && request.ToCol == request.FromCol + 2)
            {
                if (_game.Board[request.FromRow + direction][request.FromCol + 1] != null &&
                    _game.Board[request.FromRow + direction][request.FromCol + 1].player != _game.CurrentTurn &&
                    _game.Board[request.ToRow][request.ToCol] == null)
                {
                    captured = true;
                    return true;
                }
            }

            if (request.ToRow == request.FromRow + 2 * direction && request.ToCol == request.FromCol - 2)
            {
                if (_game.Board[request.FromRow + direction][request.FromCol - 1] != null &&
                    _game.Board[request.FromRow + direction][request.FromCol - 1].player != _game.CurrentTurn &&
                    _game.Board[request.ToRow][request.ToCol] == null)
                {
                    captured = true;
                    return true;
                }
            }

            return false; // Invalid move
        }


        private bool CheckGameEnd()
        {
            return Player1Pieces == 0 || Player2Pieces == 0;

        }

        [HttpPost("reset")]
        public IActionResult ResetGame()
        {
            RestartGame();
            return Ok(new
            {
                board = _game.Board,  // Reset board state
                currentTurn = _game.CurrentTurn,  // Reset player turn
                message = "Game has been reset."  // Optional message
            });
        }
        private void RestartGame()
        {
            _game = new Game();
            Player1Pieces = 12;
            Player2Pieces = 12;
            _logger.LogInformation("Game restarted.");
        }
    }

    public class MoveRequest
    {
        public int FromRow { get; set; }
        public int FromCol { get; set; }
        public int ToRow { get; set; }
        public int ToCol { get; set; }
    }
}
