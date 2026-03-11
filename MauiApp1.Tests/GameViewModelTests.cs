using MauiApp1.Models;
using MauiApp1.Services;
using MauiApp1.ViewModels;
using MauiApp1.Tests.Fakes;

namespace MauiApp1.Tests;

public class GameViewModelTests
{
    private GameViewModel CreateViewModel(int botMove = 1)
    {
        var bot = new FakeBotService(botMove);
        var history = new FakeHistoryService();
        var engine = new GameEngine();

        return new GameViewModel(bot, history, engine);
    }

    [Fact]
    public void ResetGame_Should_Create_9_Empty_Cells()
    {
        var vm = CreateViewModel();

        Assert.Equal(9, vm.Cells.Count);
        Assert.All(vm.Cells, c => Assert.Equal("", c.Value));
        Assert.Equal("Tour: Humain (X)", vm.Status);
        Assert.All(vm.Cells, c => Assert.True(c.IsEnabled));
    }

    [Fact]
    public async Task Play_Should_Set_Human_Move_Then_Bot_Move()
    {
        var vm = CreateViewModel(botMove: 1);

        await vm.PlayCommand.ExecuteAsync(vm.Cells[0]);

        Assert.Equal("X", vm.Cells[0].Value);
        Assert.Equal("O", vm.Cells[1].Value);
    }

    [Fact]
    public void ResetGame_Should_Clear_Board()
    {
        var vm = CreateViewModel();

        vm.Cells[0].Value = "X";
        vm.Cells[1].Value = "O";
        vm.Cells[0].IsEnabled = false;

        vm.ResetGameCommand.Execute(null);

        Assert.All(vm.Cells, c =>
        {
            Assert.Equal("", c.Value);
            Assert.True(c.IsEnabled);
        });

        Assert.Equal("Tour: Humain (X)", vm.Status);
    }
}