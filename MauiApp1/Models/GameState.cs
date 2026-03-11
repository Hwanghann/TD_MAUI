namespace MauiApp1.Models;

public class GameState
{
    public string CurrentPlayer { get; set; } = "X";
    public bool IsGameOver { get; set; }
    public int HumanScore { get; set; }
    public int BotScore { get; set; }
}
