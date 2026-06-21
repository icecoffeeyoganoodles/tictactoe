using System;
using System.Collections.Generic;
using TicTacToe.Desktop.Enums; // ou .Models selon ton organisation

namespace TicTacToe.Desktop.Models;

public class GameEngine
{
    public Board GameBoard { get; }
    public CellState CurrentPlayer { get; private set; }
    public CellState HumanPlayer { get; private set; }
    public GameState Status { get; private set; }
    public int WinCondition { get; }

    public GameEngine(int boardSize, int winCondition, CellState humanPlayerChoice)
    {
        GameBoard = new Board(boardSize);
        WinCondition = winCondition;
        HumanPlayer = humanPlayerChoice;

        CurrentPlayer = CellState.X;
        Status = GameState.InProgress;
    }

    public void StartGame()
    {
        Status = GameState.InProgress;
        CurrentPlayer = CellState.X;

        // premier tour ordi si humain = ronds
        if (HumanPlayer == CellState.O)
        {
            PlayComputerTurn();
        }
    }

    public bool PlayTurn(int row, int col)
    {
        // blocage si fin ou mauvais tour
        if (Status != GameState.InProgress || CurrentPlayer != HumanPlayer)
            return false;

        // tentative placement
        if (GameBoard.PlaceMark(row, col, CurrentPlayer))
        {
            // verif victoire ou egalite
            UpdateGameStatus(row, col);

            // suite jeu
            if (Status == GameState.InProgress)
            {
                SwitchPlayer();
                PlayComputerTurn();
            }
            return true;
        }

        return false;
    }

    private void SwitchPlayer()
    {
        CurrentPlayer = (CurrentPlayer == CellState.X) ? CellState.O : CellState.X;
    }

    private void PlayComputerTurn()
    {
        // securite statut
        if (Status != GameState.InProgress)
            return;

        // recherche cases vides
        var emptyCells = new List<(int row, int col)>();
        for (int r = 0; r < GameBoard.Size; r++)
        {
            for (int c = 0; c < GameBoard.Size; c++)
            {
                if (GameBoard.GetCellAt(r, c) == CellState.Empty)
                {
                    emptyCells.Add((r, c));
                }
            }
        }

        // securite grille pleine
        if (emptyCells.Count == 0)
            return;

        // tirage au sort
        Random rand = new Random();
        var index = rand.Next(emptyCells.Count);
        var chosenCell = emptyCells[index];

        // action
        GameBoard.PlaceMark(chosenCell.row, chosenCell.col, CurrentPlayer);

        // bilan
        UpdateGameStatus(chosenCell.row, chosenCell.col);

        // fin de tour
        if (Status == GameState.InProgress)
        {
            SwitchPlayer();
        }
    }

    private void UpdateGameStatus(int lastRow, int lastCol)
    {
        // test victoire
        if (CheckWin(lastRow, lastCol))
        {
            Status = (CurrentPlayer == CellState.X) ? GameState.XWins : GameState.OWins;
            return;
        }

        // test egalite
        if (IsBoardFull())
        {
            Status = GameState.Draw;
        }
    }

    private bool CheckWin(int row, int col)
    {
        // axes : horizontal, vertical, diag 1, diag 2
        int[][] directions =
        {
            new int[] { 0, 1 },
            new int[] { 1, 0 },
            new int[] { 1, 1 },
            new int[] { 1, -1 },
        };

        foreach (var dir in directions)
        {
            // init a 1 (pion actuel)
            int count = 1;

            // balayage axe positif
            count += CountMarksInDirection(row, col, dir[0], dir[1]);
            // balayage axe negatif
            count += CountMarksInDirection(row, col, -dir[0], -dir[1]);

            // condition atteinte
            if (count >= WinCondition)
                return true;
        }

        return false;
    }

    private int CountMarksInDirection(int startRow, int startCol, int dRow, int dCol)
    {
        int count = 0;
        int r = startRow + dRow;
        int c = startCol + dCol;

        // boucle sur alignement meme couleur
        while (
            r >= 0
            && r < GameBoard.Size
            && c >= 0
            && c < GameBoard.Size
            && GameBoard.GetCellAt(r, c) == CurrentPlayer
        )
        {
            count++;
            r += dRow;
            c += dCol;
        }

        return count;
    }

    private bool IsBoardFull()
    {
        // scan total
        for (int r = 0; r < GameBoard.Size; r++)
        {
            for (int c = 0; c < GameBoard.Size; c++)
            {
                // case vide trouvee
                if (GameBoard.GetCellAt(r, c) == CellState.Empty)
                    return false;
            }
        }

        // grille pleine
        return true;
    }
}
