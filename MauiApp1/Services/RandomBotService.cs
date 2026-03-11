using System;
using System.Linq;

namespace MauiApp1.Services;

public class RandomBotService : IBotService
{
    static readonly int[][] Wins =
    {
        new[]{0,1,2}, new[]{3,4,5}, new[]{6,7,8},
        new[]{0,3,6}, new[]{1,4,7}, new[]{2,5,8},
        new[]{0,4,8}, new[]{2,4,6}
    };

    public int ChooseMove(string[] board, string botSymbol, string humanSymbol)
    {
        // 1) Try win
        var winMove = FindCompletingMove(board, botSymbol);
        if (winMove != -1) return winMove;

        // 2) Try block
        var blockMove = FindCompletingMove(board, humanSymbol);
        if (blockMove != -1) return blockMove;

        // 3) Take center
        if (board[4] == "") return 4;

        // 4) Random among free
        var free = board.Select((v, i) => (v, i)).Where(x => x.v == "").Select(x => x.i).ToList();
        if (free.Count == 0) return -1;

        return free[Random.Shared.Next(free.Count)];
    }

    int FindCompletingMove(string[] board, string symbol)
    {
        foreach (var w in Wins)
        {
            var a = w[0]; var b = w[1]; var c = w[2];
            // two same + one empty
            if (board[a] == symbol && board[b] == symbol && board[c] == "") return c;
            if (board[a] == symbol && board[c] == symbol && board[b] == "") return b;
            if (board[b] == symbol && board[c] == symbol && board[a] == "") return a;
        }
        return -1;
    }
}