using SQLite;
using System.Text.Json;

namespace StraightScorer.Core.Models;

public class MatchResult
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public DateTime MatchDate { get; set; } = DateTime.Now;
    public string PlayersJson { get; set; } = "";

    [Ignore]
    public List<PlayerMatchSummary> Players
    {
        get => string.IsNullOrEmpty(PlayersJson)
            ? []
            : JsonSerializer.Deserialize<List<PlayerMatchSummary>>(PlayersJson) ?? new();
        set => PlayersJson = JsonSerializer.Serialize(value);
    }

    [Ignore]
    public string WinnerName => Players.MaxBy(p => p.FinalScore)?.Name ?? "Unknown";
}

public class PlayerMatchSummary
{
    public string Name { get; set; } = "";
    public int FinalScore { get; set; }
    public float AverageBreak { get; set; }
    public int HighestBreak { get; set; }
    public int TotalFouls { get; set; }
}