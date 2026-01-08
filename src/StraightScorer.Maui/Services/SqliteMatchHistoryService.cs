using SQLite;
using StraightScorer.Core.Models;
using StraightScorer.Core.Services.Interfaces;

namespace StraightScorer.Maui.Services;

public class SqliteMatchHistoryService : IMatchHistoryService
{
    private SQLiteAsyncConnection? _database;

    private async Task Init()
    {
        if (_database is not null)
            return;

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "MatchHistory.db3");
        _database = new SQLiteAsyncConnection(dbPath);
        await _database.CreateTableAsync<MatchResult>();
    }

    public async Task SaveMatchResultAsync(MatchResult matchResult)
    {
        await Init();
        await _database!.InsertAsync(matchResult);
    }
    public async Task<List<MatchResult>> GetMatchesAsync()
    {
        await Init();
        return await _database!.Table<MatchResult>().OrderByDescending(m => m.MatchDate).ToListAsync();
    }

    public async Task DeleteMatchResultAsync(MatchResult matchResult)
    {
        await Init();
        await _database!.DeleteAsync(matchResult);
    }

    public async Task ClearMatchHistoryAsync()
    {
        await Init();
        await _database!.DeleteAllAsync<MatchResult>();
    }
}
