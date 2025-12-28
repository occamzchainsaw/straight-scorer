using StraightScorer.Maui.Models;

namespace StraightScorer.Maui.Services;

public class GameSession
{
    public List<Player> Players { get; set; } = [];
    public int BallsOnTable { get; set; } = 15;
    public int PlayerAtTableIndex { get; set; }
    public int StartingPlayerIndex { get; set; }
    public int TargetScore { get; set; }
}