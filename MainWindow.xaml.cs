using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TicTacToe.Desktop.Models;
using TicTacToe.Desktop.Enums;

namespace TicTacToe.Desktop;

public partial class MainWindow : Window
{
    private GameEngine _engine;
    private int _boardSize = 3;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void StartGame_Click(object sender, RoutedEventArgs e)
    {
        // recup choix joueur
        CellState humanChoice = RadioX.IsChecked == true ? CellState.X : CellState.O;

        // creation moteur
        _engine = new GameEngine(_boardSize, 3, humanChoice);

        // blocage menu
        ConfigPanel.IsEnabled = false;

        // demarrage
        _engine.StartGame();

        // init ecran
        DrawGrid();
        UpdateUI();
    }

    private void DrawGrid()
    {
        // nettoyage
        GameGrid.Children.Clear();

        // dimensions
        GameGrid.Rows = _boardSize;
        GameGrid.Columns = _boardSize;

        // creation boutons dynamique
        for (int r = 0; r < _boardSize; r++)
        {
            for (int c = 0; c < _boardSize; c++)
            {
                Button btn = new Button
                {
                    FontSize = 32,
                    FontWeight = FontWeights.Bold,
                    Background = Brushes.White,
                    // stockage securise des coordonnees
                    Tag = new Tuple<int, int>(r, c),
                };

                // lien clic
                btn.Click += Cell_Click;

                // ajout a la grille
                GameGrid.Children.Add(btn);
            }
        }
    }

    private void Cell_Click(object sender, RoutedEventArgs e)
    {
        // recup bouton et coordonnees
        Button clickedBtn = sender as Button;
        var position = (Tuple<int, int>)clickedBtn.Tag;

        // transmission au moteur
        bool success = _engine.PlayTurn(position.Item1, position.Item2);

        // rafraichissement si coup valide
        if (success)
        {
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        // mise a jour affichage cases
        int index = 0;
        for (int r = 0; r < _boardSize; r++)
        {
            for (int c = 0; c < _boardSize; c++)
            {
                Button btn = GameGrid.Children[index] as Button;
                CellState state = _engine.GameBoard.GetCellAt(r, c);

                if (state == CellState.X)
                    btn.Content = "X";
                else if (state == CellState.O)
                    btn.Content = "O";
                else
                    btn.Content = "";

                index++;
            }
        }

        // gestion fin de partie
        if (_engine.Status == GameState.XWins)
        {
            StatusText.Text = "les croix ont gagné !";
            ShowRestart();
        }
        else if (_engine.Status == GameState.OWins)
        {
            StatusText.Text = "les ronds ont gagné !";
            ShowRestart();
        }
        else if (_engine.Status == GameState.Draw)
        {
            StatusText.Text = "égalité parfaite !";
            ShowRestart();
        }
        else
        {
            StatusText.Text = "partie en cours...";
        }
    }

    private void ShowRestart()
    {
        // affichage bouton rejouer
        RestartButton.Visibility = Visibility.Visible;
    }

    private void Restart_Click(object sender, RoutedEventArgs e)
    {
        // remise a zero
        RestartButton.Visibility = Visibility.Collapsed;
        ConfigPanel.IsEnabled = true;
        StatusText.Text = "choisissez un pion pour commencer";
        GameGrid.Children.Clear();
    }
}
