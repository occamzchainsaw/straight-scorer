using StraightScorer.Core.Models;

namespace StraightScorer.Core.Services.Interfaces;

public interface IMatchHistoryService
{
    Task SaveMatchResultAsync(MatchResult matchResult);
    Task<List<MatchResult>> GetMatchesAsync();
    Task DeleteMatchResultAsync(MatchResult matchResult);
    Task ClearMatchHistoryAsync();
}
