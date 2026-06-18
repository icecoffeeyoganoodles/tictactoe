using System;
using TicTacToe.Console.Enums;

namespace TicTacToe.Console.Models
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
    }
}
