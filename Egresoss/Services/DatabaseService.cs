using Egresoss.Models;
using SQLite;

namespace Egresoss.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection _database;

    async Task Init()
    {
        if (_database is not null) return;

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "Finanzas.db3");
        _database = new SQLiteAsyncConnection(dbPath);

        await _database.CreateTableAsync<Wallet>();
        await _database.CreateTableAsync<Transaction>();
        await _database.CreateTableAsync<CategoryBudget>();
    }

    public async Task<int> SaveTransactionAsync(Transaction item)
    {
        await Init();
        // Lógica de negocio: Actualizar saldo de la Wallet
        var wallet = await _database.Table<Wallet>().FirstOrDefaultAsync(w => w.Id == item.WalletId);
        if (wallet != null)
        {
            if (item.IsIncome) wallet.CurrentBalance += item.Amount;
            else wallet.CurrentBalance -= item.Amount;
            await _database.UpdateAsync(wallet);
        }
        return await _database.InsertAsync(item);
    }

    public async Task<List<Transaction>> GetTransactionsAsync()
    {
        await Init();
        return await _database.Table<Transaction>().OrderByDescending(t => t.Date).ToListAsync();
    }
}