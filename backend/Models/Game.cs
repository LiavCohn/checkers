using System.Collections.Generic;

namespace CheckersApi.Models
{
    public class Game
    {
        private const int BoardSize = 8;
        public string CurrentTurn { get; set; }
        public List<List<Checker>> Board { get; set; }

        public Game()
        {
            CurrentTurn = "player1";
            Board = CreateInitialBoard();
        }

        private List<List<Checker>> CreateInitialBoard()
        {
            var board = new List<List<Checker>>(BoardSize);
            for (int i = 0; i < BoardSize; i++)
            {
                board.Add(new List<Checker>(BoardSize));
                for (int j = 0; j < BoardSize; j++)
                {
                    if ((i + j) % 2 == 1 && i < 3) //places in the beggining 3 rows of player1
                        board[i].Add(new Checker { player = "player1" });
                    else if ((i + j) % 2 == 1 && i > 4) //places in the end 3 rows of player2
                        board[i].Add(new Checker { player = "player2" });
                    else //empty spaces
                        board[i].Add(null);
                }
            }
            return board;
        }
    }

    public class Checker
    {
        public string player { get; set; }
    }
}
