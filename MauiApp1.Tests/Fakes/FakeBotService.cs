using MauiApp1.Services;

namespace MauiApp1.Tests.Fakes;

public class FakeBotService : IBotService
{
    private readonly int moveToPlay;

    public FakeBotService(int moveToPlay)
    {
        this.moveToPlay = moveToPlay;
    }

    public int ChooseMove(string[] board, string botSymbol, string humanSymbol)
    {
        return moveToPlay;
    }
}