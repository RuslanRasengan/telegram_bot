using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Messanging_Bot
{
    public class TicTacToeGame
    {
        private readonly char[,] _board = new char[3, 3];

        private char _currentPlayer;

        public TicTacToeGame()
        {
            _currentPlayer = 'X'; //Гра починається з гравця "Х"

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    _board[i, j] = ' ';
                }
            }
        }

        public string MakeMove(int row, int column)
        {
            if (_board[row, column] != ' ')
            {
                return "Ця клітинка вже зайнята!";
            }
            _board[row, column] = _currentPlayer;

            if (CheckWinner(_currentPlayer))
            {
                return $"{_currentPlayer} виграв!";
            }

            if (_currentPlayer = _currentPlayer == 'X' ? '0' : 'X';
            {
                return null; //Немає переможця, гра продовжєуться
            }
        }

        
    }
}
