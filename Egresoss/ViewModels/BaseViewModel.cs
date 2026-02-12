using CommunityToolkit.Mvvm.ComponentModel;

namespace Egresoss.ViewModels;

// ¡IMPORTANTE! Debe ser partial
public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isBusy; // Generará IsBusy

    [ObservableProperty]
    private string title; // Generará Title
}