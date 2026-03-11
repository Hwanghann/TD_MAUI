using MauiApp1.Models;

namespace MauiApp1.Services;

public interface IGamePersistenceService
{
    void Save(GameSaveData data);
    GameSaveData? Load();
    void Clear();
}