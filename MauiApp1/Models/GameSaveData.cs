namespace MauiApp1.Models;

public class GameSaveData
{
    public List<string> Board { get; set; } = new();
    public List<GameHistoryItem> History { get; set; } = new();

    public string Status { get; set; } = "Tour: Humain (X)";
    public bool IsGameOver { get; set; }

    public int HumanScore { get; set; }
    public int BotScore { get; set; }
}