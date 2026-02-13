using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Egresoss.Models;
using Egresoss.Services;
using System.Collections.ObjectModel;

namespace Egresoss.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    private readonly DatabaseService _dbService;

    public ObservableCollection<Transaction> RecentTransactions { get; } = new();
    public ObservableCollection<ExpenseByCategory> ExpensesByCategory { get; } = new();

    // ✅ NUEVA: Colección para ingresos por categoría
    [ObservableProperty] private ObservableCollection<ExpenseByCategory> incomeByCategory = new();

    [ObservableProperty] private decimal totalBalance;
    [ObservableProperty] private decimal totalIncome;
    [ObservableProperty] private decimal totalExpense;
    [ObservableProperty] private double incomeProgress;
    [ObservableProperty] private double expenseProgress;

    public MainViewModel(DatabaseService dbService)
    {
        _dbService = dbService;
        Title = "Mis Finanzas";
    }

    [RelayCommand]
    public async Task LoadData()
    {
        IsBusy = true;

        var transactions = (await _dbService.GetTransactionsAsync()).ToList();

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

        // ✅ NUEVO: Calcular ingresos por categoría
        var incomeTx = transactions.Where(t => t.IsIncome);
        var incomeGrouped = incomeTx
            .GroupBy(t => t.Category ?? "Otros Ingresos")
            .Select(g => new ExpenseByCategory
            {
                Category = g.Key,
                Amount = g.Sum(x => x.Amount)
            })
            .OrderByDescending(x => x.Amount)
            .ToList();

        decimal totalIncomeAmount = incomeGrouped.Sum(g => g.Amount);

        incomeByCategory.Clear();
        foreach (var g in incomeGrouped)
        {
            g.Percentage = totalIncomeAmount > 0 ? (double)(g.Amount / totalIncomeAmount) : 0;
            incomeByCategory.Add(g);
        }

        // --- Construir barras proporcionales ---
        BuildBars(transactions);

        IsBusy = false;
    }

    private void BuildBars(List<Transaction> transactions)
    {
        TotalIncome = transactions.Where(t => t.IsIncome).Sum(t => t.Amount);
        TotalExpense = transactions.Where(t => !t.IsIncome).Sum(t => t.Amount);

        double max = (double)Math.Max(TotalIncome, TotalExpense);

        IncomeProgress = max > 0 ? (double)TotalIncome / max : 0;
        ExpenseProgress = max > 0 ? (double)TotalExpense / max : 0;
    }
}