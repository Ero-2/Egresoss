using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Egresoss.Models;
using Egresoss.Services;
using System.Collections.ObjectModel;
using System.Data;

namespace Egresoss.ViewModels;

public partial class TransactionViewModel : BaseViewModel
{
    private readonly DatabaseService _dbService;

    [ObservableProperty] private string amountText;
    [ObservableProperty] private string selectedCategory;
    [ObservableProperty] private DateTime selectedDate = DateTime.Now;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CategoriesList))]
    private bool isIncome;

    // Propiedad calculada para el Picker
    public string[] CategoriesList => IsIncome ? Categories.IncomeCategories : Categories.ExpenseCategories;

    public TransactionViewModel(DatabaseService dbService)
    {
        _dbService = dbService;
        Title = "Nuevo Registro";
        IsIncome = false;

        // Inicialización segura de la categoría
        if (CategoriesList.Length > 0)
            SelectedCategory = CategoriesList[0];
    }

    [RelayCommand]
    private void SetType(string type)
    {
        IsIncome = (type == "income");

        // Al cambiar de tipo, reseteamos la categoría a la primera de la nueva lista
        if (CategoriesList.Length > 0)
            SelectedCategory = CategoriesList[0];
    }

    [RelayCommand]
    private async Task SaveTransaction()
    {
        if (string.IsNullOrWhiteSpace(AmountText))
        {
            await Shell.Current.DisplayAlert("Aviso", "Por favor ingresa un monto", "OK");
            return;
        }

        try
        {
            decimal finalAmount = EvaluateExpression(AmountText);

            if (finalAmount <= 0)
            {
                await Shell.Current.DisplayAlert("Error", "El monto debe ser mayor a 0", "OK");
                return;
            }

            var newTransaction = new Transaction
            {
                Amount = finalAmount,
                IsIncome = IsIncome,
                Category = SelectedCategory ?? "Otros",
                Date = SelectedDate,
                WalletId = 1
            };

            // Esperamos a que la DB confirme el guardado
            await _dbService.SaveTransactionAsync(newTransaction);

            // Volvemos a la pantalla anterior (el OnAppearing de MainPage hará el resto)
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"No se pudo guardar: {ex.Message}", "OK");
        }
    }

    private decimal EvaluateExpression(string expression)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(expression)) return 0;

            DataTable table = new DataTable();
            // Reemplazamos coma por punto para que el motor matemático no falle
            string cleanExpression = expression.Replace(",", ".");
            var result = table.Compute(cleanExpression, "");
            return Convert.ToDecimal(result);
        }
        catch
        {
            return 0;
        }
    }
}