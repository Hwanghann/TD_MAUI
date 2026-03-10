using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using MauiApp1.Models;

namespace MauiApp1.ViewModels;

public partial class GameViewModel : ObservableObject
{
    public ObservableCollection<CellViewModel> Cells { get; } = new();

    [ObservableProperty]
    string status = "Tour du joueur X";

    string currentPlayer = "X";
    bool gameOver = false;

    int[,] wins =
{
    {0,1,2},{3,4,5},{6,7,8},
    {0,3,6},{1,4,7},{2,5,8},
    {0,4,8},{2,4,6}
};

    public GameViewModel()
    {
        ResetGame();
    }

    [RelayCommand]
    void Play(CellViewModel cell)
    {
        if (gameOver || cell.Value != "")
            return;

        cell.Value = currentPlayer;

        if (CheckWinner())
        {
            Status = $"Le joueur {currentPlayer} gagne !";
            gameOver = true;
            return;
        }

        if (Cells.All(c => c.Value != ""))
        {
            Status = "Match nul";

            foreach (var c in Cells)
            {
                c.BackgroundColor = Colors.LightGray;
                c.IsEnabled = false;
            }

            return;
        }

        currentPlayer = currentPlayer == "X" ? "O" : "X";
        Status = $"Tour du joueur {currentPlayer}";
    }

    [RelayCommand]
    void ResetGame()
    {
        Cells.Clear();

        for (int i = 0; i < 9; i++)
        {
            Cells.Add(new CellViewModel(new GameCell()));
        }

        currentPlayer = "X";
        Status = "Tour du joueur X";
        gameOver = false;
    }

    bool CheckWinner()
    {
        string[] board = Cells.Select(c => c.Value).ToArray();

        for (int i = 0; i < wins.GetLength(0); i++)
        {
            int a = wins[i, 0];
            int b = wins[i, 1];
            int c = wins[i, 2];

            if (board[a] != "" &&
                board[a] == board[b] &&
                board[b] == board[c])
            {
                Cells[a].BackgroundColor = Colors.GreenYellow;
                Cells[b].BackgroundColor = Colors.GreenYellow;
                Cells[c].BackgroundColor = Colors.GreenYellow;

                DisableBoard();

                return true;
            }
        }

        return false;
    }

    void DisableBoard()
    {
        foreach (var cell in Cells)
        {
            cell.IsEnabled = false;
        }
    }
}