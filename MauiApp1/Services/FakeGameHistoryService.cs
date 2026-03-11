using System.Collections.ObjectModel;
using MauiApp1.Models;

namespace MauiApp1.Services;

public class FakeGameHistoryService : IGameHistoryService
{
    public ObservableCollection<GameHistoryItem> Items { get; } = new();

    public void Add(GameHistoryItem item)
    {
        Items.Insert(0, item);
    }
}