using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;
using MauiApp1.Models;
using MauiApp1.Services;

namespace MauiApp1.ViewModels;

public partial class GameViewModel : ObservableObject
{
    private readonly IBotService bot;
    private readonly IGameHistoryService history;
    private readonly IGameEngine engine;
    private readonly IGamePersistenceService persistence;

    public ObservableCollection<CellViewModel> Cells { get; } = new();
    public ObservableCollection<GameHistoryItem> History => history.Items;

    [ObservableProperty]
    private string status = "Tour: Humain (X)";

    [ObservableProperty]
    private bool isBusy;

    private const string Human = "X";
    private const string Bot = "O";

    private bool gameOver = false;
    private int humanScore = 0;
    private int botScore = 0;

    private static readonly Color DefaultCellColor = Color.FromArgb("#512BD4");
    private static readonly Color WinCellColor = Color.FromArgb("#236109");
    private static readonly Color LoseBoardColor = Color.FromArgb("#3A3A3A");

    public GameViewModel(
        IBotService bot,
        IGameHistoryService history,
        IGameEngine engine,
        IGamePersistenceService persistence)
    {
        this.bot = bot;
        this.history = history;
        this.engine = engine;
        this.persistence = persistence;

        LoadGame();
    }

    [RelayCommand]
    private async Task Play(CellViewModel cell)
    {
        if (gameOver || IsBusy || !cell.IsEnabled || cell.Value != "")
            return;

        ApplyMove(cell, Human);

        if (TryEndGameFromBoard(Human))
            return;

        IsBusy = true;
        Status = "Tour: Bot (O)";
        SaveGame();

        await Task.Delay(250);

        var board = GetBoard();
        int move = bot.ChooseMove(board, Bot, Human);

        if (move >= 0 && move < Cells.Count && Cells[move].Value == "")
        {
            ApplyMove(Cells[move], Bot);
        }

        IsBusy = false;

        if (TryEndGameFromBoard(Bot))
            return;

        Status = "Tour: Humain (X)";
        SaveGame();
    }

    [RelayCommand]
    private void ResetGame()
    {
        gameOver = false;
        IsBusy = false;
        Status = "Tour: Humain (X)";

        Cells.Clear();

        for (int i = 0; i < 9; i++)
        {
            Cells.Add(new CellViewModel(new GameCell())
            {
                Value = "",
                IsEnabled = true,
                BackgroundColor = DefaultCellColor
            });
        }

        SaveGame();
    }

    private void ApplyMove(CellViewModel cell, string symbol)
    {
        cell.Value = symbol;
        SaveGame();
    }

    private bool TryEndGameFromBoard(string lastPlayer)
    {
        var board = GetBoard();

        if (engine.TryFindWinningLine(board, out var line))
        {
            gameOver = true;
            HighlightWinner(line);
            DisableAllCells();

            if (lastPlayer == Human)
            {
                humanScore++;
                Status = "✅ Victoire de l'humain";
                history.Add(new GameHistoryItem
                {
                    Date = DateTime.Now,
                    HumanScore = humanScore,
                    BotScore = botScore,
                    Result = GameResult.Victory
                });
            }
            else
            {
                botScore++;
                Status = "❌ Victoire du bot";
                history.Add(new GameHistoryItem
                {
                    Date = DateTime.Now,
                    HumanScore = humanScore,
                    BotScore = botScore,
                    Result = GameResult.Defeat
                });
            }

            SaveGame();
            return true;
        }

        if (engine.IsDraw(board))
        {
            gameOver = true;
            GrayOutDraw();
            DisableAllCells();
            Status = "🤝 Match nul";

            history.Add(new GameHistoryItem
            {
                Date = DateTime.Now,
                HumanScore = humanScore,
                BotScore = botScore,
                Result = GameResult.Draw
            });

            SaveGame();
            return true;
        }

        return false;
    }

    private string[] GetBoard()
    {
        return Cells.Select(c => c.Value).ToArray();
    }

    private void HighlightWinner(int[] line)
    {
        foreach (var cell in Cells)
        {
            cell.BackgroundColor = LoseBoardColor;
        }

        foreach (var index in line)
        {
            Cells[index].BackgroundColor = WinCellColor;
        }
    }

    private void GrayOutDraw()
    {
        foreach (var cell in Cells)
        {
            cell.BackgroundColor = Colors.LightGray;
        }
    }

    private void DisableAllCells()
    {
        foreach (var cell in Cells)
        {
            cell.IsEnabled = false;
        }
    }

    private void SaveGame()
    {
        var data = new GameSaveData
        {
            Board = Cells.Select(c => c.Value).ToList(),
            History = history.Items.ToList(),
            Status = Status,
            IsGameOver = gameOver,
            HumanScore = humanScore,
            BotScore = botScore
        };

        persistence.Save(data);
    }

    private void LoadGame()
    {
        var data = persistence.Load();

        if (data == null)
        {
            ResetGame();
            return;
        }

        Cells.Clear();

        foreach (var value in data.Board)
        {
            Cells.Add(new CellViewModel(new GameCell())
            {
                Value = value,
                IsEnabled = !data.IsGameOver && value == "",
                BackgroundColor = DefaultCellColor
            });
        }

        while (Cells.Count < 9)
        {
            Cells.Add(new CellViewModel(new GameCell())
            {
                Value = "",
                IsEnabled = !data.IsGameOver,
                BackgroundColor = DefaultCellColor
            });
        }

        Status = data.Status;
        gameOver = data.IsGameOver;
        humanScore = data.HumanScore;
        botScore = data.BotScore;

        history.Items.Clear();
        foreach (var item in data.History)
        {
            history.Items.Add(item);
        }

        if (gameOver)
        {
            foreach (var cell in Cells)
            {
                cell.IsEnabled = false;
            }
        }
    }
}