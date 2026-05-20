using MauiApp1.Models;

namespace MauiApp1.Services;

public interface IApiGameService
{
    Task<ApiGame> CreateGameAsync();
    Task<ApiGame> GetGameAsync(Guid id);
    Task<ApiGame> PlayMoveAsync(Guid id, int position);
}