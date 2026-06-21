using System;
using TicTacToe.Desktop.Enums;

namespace TicTacToe.Desktop.Models
{
    public class Board
    {
        public int Size { get; set; }
        private readonly CellState[,] _cells;

        public Board(int size)
        {
            Size = size;
            _cells = new CellState[size, size];
        }

        public CellState GetCellAt(int row, int col)
        {
            return _cells[row, col];
        }

        public bool PlaceMark(int row, int col, CellState playerMark)
        {
            // hors limites
            if (row < 0 || row >= Size || col < 0 || col >= Size)
                return false;

            // case occupee
            if (_cells[row, col] != CellState.Empty)
                return false;

            // placement valide
            _cells[row, col] = playerMark;
            return true;
        }
    }
}
