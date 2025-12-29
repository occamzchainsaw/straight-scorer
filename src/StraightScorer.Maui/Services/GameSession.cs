using StraightScorer.Maui.Models;

namespace StraightScorer.Maui.Services;

public class GameSession
{
    public Player Player1 { get; set; } = new();
    public Player Player2 { get; set; } = new();
    public int BallsOnTable { get; set; } = 15;
    public int TargetScore { get; set; }
    public Player? PlayerAtTable { get; set; } = null;

    //todo: add break history as List<Break>
}