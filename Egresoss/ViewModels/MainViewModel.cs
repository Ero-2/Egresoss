using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Egresoss.Models;
using Egresoss.Services;
using Microcharts;
using SkiaSharp;
using System.Collections.ObjectModel;

namespace Egresoss.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    private readonly DatabaseService _dbService;

    public ObservableCollection<Transaction> RecentTransactions { get; } = new();
    public ObservableCollection<ExpenseByCategory> ExpensesByCategory { get; } = new();

    [ObservableProperty]
    private decimal totalBalance;

    [ObservableProperty]
    private Chart summaryChart; // 👈 AGREGADO

    // Paleta de colores para la gráfica
    private readonly string[] _chartColors = new[]
    {
        "#6C63FF", "#F44336", "#FF9800", "#4CAF50",
        "#2196F3", "#9C27B0", "#00BCD4", "#FF5722"
    };

    public MainViewModel(DatabaseService dbService)
    {
        _dbService = dbService;
        Title = "Mis Finanzas";
    }

    [RelayCommand]
    public async Task LoadData()
    {
        IsBusy = true;

        var transactions = await _dbService.GetTransactionsAsync();

        RecentTransactions.Clear();
        decimal balance = 0;

        foreach (var t in transactions)
        {
            RecentTransactions.Add(t);
            balance += t.IsIncome ? t.Amount : -t.Amount;
        }

        TotalBalance = balance;

        // --- Calcular gastos por categoría ---
        var expenseTx = transactions.Where(t => !t.IsIncome);
        var grouped = expenseTx
            .GroupBy(t => t.Category ?? "Otros")
            .Select(g => new ExpenseByCategory
            {
                Category = g.Key,
                Amount = g.Sum(x => x.Amount)
            })
            .OrderByDescending(x => x.Amount)
            .ToList();

        decimal totalExpenses = grouped.Sum(g => g.Amount);

        ExpensesByCategory.Clear();
        foreach (var g in grouped)
        {
            g.Percentage = totalExpenses > 0 ? (double)(g.Amount / totalExpenses) : 0;
            ExpensesByCategory.Add(g);
        }

        // --- Construir gráfica ---  👈 AGREGADO
        BuildChart(transactions.ToList());

        IsBusy = false;
    }

    private void BuildChart(List<Transaction> transactions) // 👈 AGREGADO
    {
        var totalIncome = transactions.Where(t => t.IsIncome).Sum(t => t.Amount);
        var totalExpense = transactions.Where(t => !t.IsIncome).Sum(t => t.Amount);

        // Si no hay datos, no construimos la gráfica
        if (totalIncome == 0 && totalExpense == 0)
        {
            summaryChart = null;
            return;
        }

        var entries = new List<ChartEntry>();

        if (totalIncome > 0)
        {
            entries.Add(new ChartEntry((float)totalIncome)
            {
                Label = "Ingresos",
                ValueLabel = $"${totalIncome:N0}",
                Color = SKColor.Parse("#4CAF50"),
                ValueLabelColor = SKColor.Parse("#4CAF50")
            });
        }

        if (totalExpense > 0)
        {
            entries.Add(new ChartEntry((float)totalExpense)
            {
                Label = "Gastos",
                ValueLabel = $"${totalExpense:N0}",
                Color = SKColor.Parse("#F44336"),
                ValueLabelColor = SKColor.Parse("#F44336")
            });
        }

        summaryChart = new DonutChart
        {
            Entries = entries,
            LabelTextSize = 28,
            BackgroundColor = SKColors.Transparent,
            HoleRadius = 0.5f,
            LabelMode = LabelMode.RightOnly
        };
    }
}