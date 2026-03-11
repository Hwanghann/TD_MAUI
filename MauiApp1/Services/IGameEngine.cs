namespace MauiApp1.Services;

public interface IGameEngine
{
    string[] GetBoard(IEnumerable<string> values);
    bool TryFindWinningLine(string[] board, out int[] line);
    bool IsDraw(string[] board);
}