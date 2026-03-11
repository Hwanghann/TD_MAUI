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
    private readonly GameState state = new();

    public ObservableCollection<CellViewModel> Cells { get; } = new();
    public ObservableCollection<GameHistoryItem> History => history.Items;

    [ObservableProperty]
    private string status = "Tour: Humain (X)";

    [ObservableProperty]
    private bool isBusy;

    private const string Human = "X";
    private const string Bot = "O";
    private static readonly Color DefaultCellColor = Color.FromArgb("#512BD4");
    private static readonly Color WinCellColor = Color.FromArgb("#236109");
    private static readonly Color LoseBoardColor = Color.FromArgb("#3A3A3A");

    public GameViewModel(
        IBotService bot,
        IGameHistoryService history,
        IGameEngine engine)
    {
        this.bot = bot;
        this.history = history;
        this.engine = engine;

        ResetGame();
    }

    [RelayCommand]
    private async Task Play(CellViewModel cell)
    {
        if (!CanPlay(cell))
            return;

        ApplyMove(cell, Human);

        if (TryEndGame(Human))
            return;

        await PlayBotTurn();
    }

    [RelayCommand]
    private void ResetGame()
    {
        state.IsGameOver = false;
        state.CurrentPlayer = Human;
        IsBusy = false;
        Status = "Tour: Humain (X)";

        Cells.Clear();

        for (int i = 0; i < 9; i++)
        {
            var cell = new CellViewModel(new GameCell())
            {
                IsEnabled = true,
                BackgroundColor = DefaultCellColor,
                Value = ""
            };

            Cells.Add(cell);
        }
    }

    private bool CanPlay(CellViewModel cell)
    {
        return !state.IsGameOver
            && !IsBusy
            && cell.IsEnabled
            && cell.Value == "";
    }

    private void ApplyMove(CellViewModel cell, string symbol)
    {
        cell.Value = symbol;
    }

    private async Task PlayBotTurn()
    {
        IsBusy = true;
        Status = "Tour: Bot (O)";

        await Task.Delay(250);

        var board = GetBoard();
        int move = bot.ChooseMove(board, Bot, Human);

        if (move >= 0 && move < Cells.Count && Cells[move].Value == "")
        {
            ApplyMove(Cells[move], Bot);
        }

        IsBusy = false;

        if (TryEndGame(Bot))
            return;

        Status = "Tour: Humain (X)";
    }

    private bool TryEndGame(string lastPlayer)
    {
        var board = GetBoard();

        if (engine.TryFindWinningLine(board, out var line))
        {
            HandleVictory(lastPlayer, line);
            return true;
        }

        if (engine.IsDraw(board))
        {
            HandleDraw();
            return true;
        }

        return false;
    }

    private void HandleVictory(string winner, int[] line)
    {
        state.IsGameOver = true;
        HighlightWinner(line);
        DisableAllCells();

        if (winner == Human)
        {
            state.HumanScore++;
            Status = "✅ Victoire de l'humain";
            AddHistory(GameResult.Victory);
        }
        else
        {
            state.BotScore++;
            Status = "❌ Victoire du bot";
            AddHistory(GameResult.Defeat);
        }
    }

    private void HandleDraw()
    {
        state.IsGameOver = true;
        GrayOutDraw();
        DisableAllCells();
        Status = "🤝 Match nul";
        AddHistory(GameResult.Draw);
    }

    private void AddHistory(GameResult result)
    {
        history.Add(new GameHistoryItem
        {
            Date = DateTime.Now,
            HumanScore = state.HumanScore,
            BotScore = state.BotScore,
            Result = result
        });
    }

    private string[] GetBoard()
    {
        return engine.GetBoard(Cells.Select(c => c.Value));
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
}