using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Graphics;
using MauiApp1.Models;

namespace MauiApp1.ViewModels;

public partial class CellViewModel : ObservableObject
{
    public GameCell Cell { get; }

    public CellViewModel(GameCell cell)
    {
        Cell = cell;
        backgroundColor = Color.FromArgb("#512BD4"); // violet par défaut
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

    [ObservableProperty]
    private bool isEnabled = true;

    [ObservableProperty]
    private Color backgroundColor;
}