namespace Egresoss.Models;

public static class Categories
{
    public static readonly string[] ExpenseCategories =
    {
        "Alimentación", "Transporte", "Vivienda",
        "Entretenimiento", "Salud", "Ropa", "Otros"
    };

    public static readonly string[] IncomeCategories =
    {
        "Salario", "Freelance", "Inversiones", "Regalo", "Otros"
    };

    public static string GetColor(string category) => category switch
    {
        "Alimentación" => "#FF6B6B",
        "Transporte" => "#4ECDC4",
        "Vivienda" => "#45B7D1",
        "Entretenimiento" => "#96CEB4",
        "Salud" => "#FFEAA7",
        "Ropa" => "#D6A2E8",
        "Salario" => "#6C63FF",
        "Freelance" => "#FF9E6D",
        _ => "#DFE6E9"
    };
}