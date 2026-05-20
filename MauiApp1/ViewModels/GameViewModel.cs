using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Models;
using MauiApp1.Services;
using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;

namespace MauiApp1.ViewModels;

public partial class GameViewModel : ObservableObject
{
    private readonly IApiGameService apiGameService;
    private readonly IGameHistoryService history;

    private Guid currentGameId;
    private int humanScore = 0;
    private int botScore = 0;

    private static readonly Color DefaultCellColor = Color.FromArgb("#512BD4");
    private static readonly Color DisabledCellColor = Color.FromArgb("#3A3A3A");

    public ObservableCollection<CellViewModel> Cells { get; } = new();

    public ObservableCollection<GameHistoryItem> History => history.Items;

    [ObservableProperty]
    private string status = "Chargement...";

    [ObservableProperty]
    private bool isBusy;

    public GameViewModel(
        IApiGameService apiGameService,
        IGameHistoryService history)
    {
        this.apiGameService = apiGameService;
        this.history = history;

        _ = StartNewGameAsync();
    }

    [RelayCommand]
    private async Task Play(CellViewModel cell)
    {
        if (IsBusy || cell == null || !cell.IsEnabled || cell.Value != "")
            return;

        int position = Cells.IndexOf(cell);

        if (position < 0 || currentGameId == Guid.Empty)
            return;

        try
        {
            IsBusy = true;
            Status = "Le bot joue...";

            var game = await apiGameService.PlayMoveAsync(currentGameId, position);

            ApplyGameToBoard(game);
            UpdateStatus(game);
        }
        catch (Exception ex)
        {
            Status = $"Erreur API : {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ResetGame()
    {
        await StartNewGameAsync();
    }

    private async Task StartNewGameAsync()
    {
        try
        {
            IsBusy = true;
            Status = "Création de la partie...";

            var game = await apiGameService.CreateGameAsync();

            currentGameId = game.Id;

            ApplyGameToBoard(game);
            UpdateStatus(game);
        }
        catch (Exception ex)
        {
            Status = $"Erreur API : {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void ApplyGameToBoard(ApiGame game)
    {
        Cells.Clear();

        for (int i = 0; i < 9; i++)
        {
            string value = "";

            if (game.Board != null && i < game.Board.Count)
            {
                value = game.Board[i] == " " ? "" : game.Board[i];
            }

            Cells.Add(new CellViewModel(new GameCell())
            {
                Value = value,
                IsEnabled = game.Status == 0 && value == "",
                BackgroundColor = game.Status == 0 ? DefaultCellColor : DisabledCellColor
            });
        }
    }

    private void UpdateStatus(ApiGame game)
    {
        switch (game.Status)
        {
            case 0:
                Status = "Tour: Humain (X)";
                break;

            case 1:
                humanScore++;
                Status = "✅ Victoire de l'humain";
                history.Add(new GameHistoryItem
                {
                    Date = DateTime.Now,
                    HumanScore = humanScore,
                    BotScore = botScore,
                    Result = GameResult.Victory
                });
                break;

            case 2:
                botScore++;
                Status = "❌ Victoire du bot";
                history.Add(new GameHistoryItem
                {
                    Date = DateTime.Now,
                    HumanScore = humanScore,
                    BotScore = botScore,
                    Result = GameResult.Defeat
                });
                break;

            case 3:
                Status = "Match nul";
                history.Add(new GameHistoryItem
                {
                    Date = DateTime.Now,
                    HumanScore = humanScore,
                    BotScore = botScore,
                    Result = GameResult.Draw
                });
                break;
        }
    }
}