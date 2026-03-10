using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp1.Models;
using Microsoft.Maui.Graphics;

namespace MauiApp1.ViewModels;

public partial class CellViewModel : ObservableObject
{
    public GameCell Cell { get; }

    public CellViewModel(GameCell cell)
    {
        Cell = cell;
    }

    public string Value
    {
        get => Cell.Value;
        set
        {
            Cell.Value = value;
            OnPropertyChanged();
        }
    }

    Color backgroundColor = Colors.MediumPurple;
    public Color BackgroundColor
    {
        get => backgroundColor;
        set => SetProperty(ref backgroundColor, value);
    }

    bool isEnabled = true;
    public bool IsEnabled
    {
        get => isEnabled;
        set => SetProperty(ref isEnabled, value);
    }
}