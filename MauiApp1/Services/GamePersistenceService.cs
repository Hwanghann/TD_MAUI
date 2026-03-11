using System.Text.Json;
using Microsoft.Maui.Storage;
using MauiApp1.Models;

namespace MauiApp1.Services;

public class GamePersistenceService : IGamePersistenceService
{
    private const string SaveKey = "morpion_save_data";

    public void Save(GameSaveData data)
    {
        string json = JsonSerializer.Serialize(data);
        Preferences.Set(SaveKey, json);
    }

    public GameSaveData? Load()
    {
        string? json = Preferences.Get(SaveKey, null);

        if (string.IsNullOrWhiteSpace(json))
            return null;

        return JsonSerializer.Deserialize<GameSaveData>(json);
    }

    public void Clear()
    {
        Preferences.Remove(SaveKey);
    }
}