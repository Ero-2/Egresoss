namespace Egresoss.ViewModels;

public class ExpenseByCategory
{
    public string Category { get; set; } = "Otros";
    public decimal Amount { get; set; }
    // Percentage para el ProgressBar (0.0 .. 1.0)
    public double Percentage { get; set; } = 0;
}