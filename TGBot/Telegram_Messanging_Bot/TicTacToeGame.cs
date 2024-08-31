using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Messanging_Bot
{
    public class TicTacToeGame
    {
        public char[,] _board { get; private set; }

        public char _currentPlayer { get; private set; }
    
        public TicTacToeGame()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    _board[i, j] = ' ';
                    _currentPlayer = 'X';
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

            _currentPlayer = _currentPlayer == 'X' ? '0' : 'X';

            return null; //Немає переможця, гра продовжєуться
        }

        private bool CheckWinner(char player)
        {
            for (int i = 0; i < 3; i++)
            {
                if (_board[i, 0] == player && _board[i, 1] == player && _board[i, 2] == player)
                {
                    return true;
                }
                if (_board[0, i] == player && _board[1, i] == player && _board[2, i] == player)
                {
                    return true;
                }
            }
            if (_board[0, 0] == player && _board[1, 1] == player && _board[2, 2] == player)
            {
                return true;
            }
            if (_board[0, 2] == player && _board[1, 1] == player && _board[2, 0] == player)
            {
                return true;
            }
            return false;
        }

        public bool IsBoardFull()
        {
            foreach(var cell in _board)
            {
                if (cell == ' ')
                    return false;
            }
            return true;
        }

        public string GetBoard()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    sb.Append(_board[i, j]);
                    if (j < 2)
                    {
                        sb.Append("|");
                    }
                }
                sb.AppendLine();
                if (i < 2) sb.AppendLine("-+-+-");
            }
            return sb.ToString();
        }
    }
}
