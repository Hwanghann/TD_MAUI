using MauiApp1.Models;
using MauiApp1.Services;

namespace MauiApp1.Tests.Fakes;

public class FakePersistenceService : IGamePersistenceService
{
    private GameSaveData? data;

    public void Save(GameSaveData saveData)
    {
        data = saveData;
    }

    public GameSaveData? Load()
    {
        return data;
    }

    public void Clear()
    {
        data = null;
    }
}