namespace MauiApp1.Services;

public class GameEngine : IGameEngine
{
    private static readonly int[][] Wins =
    {
        new[] {0,1,2}, new[] {3,4,5}, new[] {6,7,8},
        new[] {0,3,6}, new[] {1,4,7}, new[] {2,5,8},
        new[] {0,4,8}, new[] {2,4,6}
    };

    public string[] GetBoard(IEnumerable<string> values)
    {
        return values.ToArray();
    }

    public bool TryFindWinningLine(string[] board, out int[] line)
    {
        foreach (var w in Wins)
        {
            int a = w[0];
            int b = w[1];
            int c = w[2];

            if (board[a] != "" && board[a] == board[b] && board[b] == board[c])
            {
                line = w;
                return true;
            }
        }

        line = Array.Empty<int>();
        return false;
    }

    public bool IsDraw(string[] board)
    {
        return board.All(cell => cell != "");
    }
}