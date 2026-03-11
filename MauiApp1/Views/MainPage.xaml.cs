using MauiApp1.ViewModels;

namespace MauiApp1.Views;

public partial class MainPage : ContentPage
{
    public MainPage(GameViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}