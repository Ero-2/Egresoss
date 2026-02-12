using SQLite;

namespace Egresoss.Models;

public class Wallet
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; } = "Efectivo";
    public string Icon { get; set; } = "cash";

    public decimal InitialBalance { get; set; }
    public decimal CurrentBalance { get; set; }

    public string Color { get; set; } = "#6C63FF";
    public bool IsDefault { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}