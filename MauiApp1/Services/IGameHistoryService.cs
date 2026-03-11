using System.Collections.ObjectModel;
using MauiApp1.Models;

namespace MauiApp1.Services;

public interface IGameHistoryService
{
    ObservableCollection<GameHistoryItem> Items { get; }

    void Add(GameHistoryItem item);
}