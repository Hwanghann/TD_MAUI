using System.Collections.ObjectModel;
using MauiApp1.Models;
using MauiApp1.Services;

namespace MauiApp1.Tests.Fakes;

public class FakeHistoryService : IGameHistoryService
{
    public ObservableCollection<GameHistoryItem> Items { get; } = new();

    public void Add(GameHistoryItem item)
    {
        Items.Add(item);
    }
}