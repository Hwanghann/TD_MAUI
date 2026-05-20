using System.Net.Http.Json;
using MauiApp1.Models;

namespace MauiApp1.Services;

public class ApiGameService : IApiGameService
{
#if ANDROID
    private const string ApiBaseUrl = "http://10.0.2.2:5091";
#else
    private const string ApiBaseUrl = "http://localhost:5091";
#endif

    private readonly HttpClient httpClient;

    public ApiGameService()
    {
        httpClient = new HttpClient
        {
            BaseAddress = new Uri(ApiBaseUrl)
        };
    }

    public async Task<ApiGame> CreateGameAsync()
    {
        var response = await httpClient.PostAsync("/api/games", null);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<ApiGame>()
            ?? throw new Exception("Réponse API vide.");
    }

    public async Task<ApiGame> GetGameAsync(Guid id)
    {
        var response = await httpClient.GetAsync($"/api/games/{id}");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<ApiGame>()
            ?? throw new Exception("Réponse API vide.");
    }

    public async Task<ApiGame> PlayMoveAsync(Guid id, int position)
    {
        var body = new
        {
            position = position
        };

        var response = await httpClient.PostAsJsonAsync($"/api/games/{id}/move", body);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<ApiGame>()
            ?? throw new Exception("Réponse API vide.");
    }
}