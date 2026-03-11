namespace MauiApp1.Models;

public class GameHistoryItem
{
    public DateTime Date { get; set; } = DateTime.Now;

    public int HumanScore { get; set; }
    public int BotScore { get; set; }

    public GameResult Result { get; set; }

    public string WinnerLabel => Result switch
    {
        GameResult.Victory => "Humain",
        GameResult.Defeat => "Bot",
        GameResult.Draw => "Match nul",
        _ => "Inconnu"
    };

    public string ScoreLabel => $"{HumanScore} - {BotScore}";
    public string DateLabel => Date.ToString("dd/MM/yyyy HH:mm");
}