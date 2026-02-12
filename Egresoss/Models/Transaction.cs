using SQLite;

namespace Egresoss.Models;

public class Transaction
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public decimal Amount { get; set; }
    public bool IsIncome { get; set; }

    public string Category { get; set; } = "Otros";
    public string Description { get; set; } = "";

    public int WalletId { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;

    public bool IsRecurring { get; set; } = false;
    public string RecurrencePattern { get; set; } = "";
}