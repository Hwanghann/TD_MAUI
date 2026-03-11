namespace MauiApp1.Services;

public interface IBotService
{
    int ChooseMove(string[] board, string botSymbol, string humanSymbol);
}