using StraightScorer.Core.Models;
using StraightScorer.Core.Services.Interfaces;

namespace StraightScorer.Core.Services.Commands;

public class FoulCommand(GameState _gameState) : IUndoRedoCommand
{
    private int _previousPlayerAtTableId;
    private int _nextPlayerAtTableId;
    private int _previousScore;
    private int _previousBreak;
    private int _previousHighestBreak;
    private float _previousAverageBreak;
    private int _previousBreakSum;
    private int _previousBreakCount;
    private int _previousTotalFouls;
    private int _previousConsecutiveFouls;
    private int _previousBallsOnTable;
    private int _previousRackNumber;

    public void Execute()
    {
        Player player = _gameState.GetPlayerAtTable();
        _previousPlayerAtTableId = _gameState.PlayerAtTableId;
        _nextPlayerAtTableId = _gameState.GetNextPlayerId();
        _previousScore = player.Score;
        _previousBreak = player.CurrentBreak;
        _previousHighestBreak = player.HighestBreak;
        _previousAverageBreak = player.AverageBreak;
        _previousBreakSum = player.BreakSum;
        _previousBreakCount = player.BreakCount;
        _previousTotalFouls = player.TotalFouls;
        _previousConsecutiveFouls = player.ConsecutiveFouls;
        _previousBallsOnTable = _gameState.BallsOnTable;
        _previousRackNumber = _gameState.CurrentRack;

        // foul logic
        if (player.ConsecutiveFouls == 2)
        {
            // third consecutive foul
            player.Score -= 16;
            player.ConsecutiveFouls = 0;
            player.TotalFouls++;
            _gameState.AddBreakToHistory(player.Name, -16, BreakEndAction.ThirdFoul);
            if (_gameState.GameSettings.ResetRackOnThirdFoul)
            {
                _gameState.BallsOnTable = 15;
                _gameState.CurrentRack++;
                return;
            }
        }
        else
        {
            // foul on the break shot is 2 points and not counted towards consecutive fouls
            if (_gameState.BreakHistory.Count == 0)
            {
                player.Score -= 2;
                _gameState.AddBreakToHistory(player.Name, -2, BreakEndAction.Foul);
            }
            else
            {
                player.Score--;
                player.ConsecutiveFouls++;
                _gameState.AddBreakToHistory(player.Name, player.CurrentBreak - 1, BreakEndAction.Foul);
                if (player.ConsecutiveFouls < 2)
                {
                    player.BreakCount++;
                    player.BreakSum += player.CurrentBreak;
                    player.AverageBreak = (float)player.BreakSum / (float)player.BreakCount;
                    player.HighestBreak = Math.Max(player.CurrentBreak, player.HighestBreak);
                }
            }
            player.TotalFouls++;
            player.CurrentBreak = 0;
        }

        player.IsAtTable = false;
        _gameState.PlayerAtTableId = _nextPlayerAtTableId;
        // change reference to next player
        player = _gameState.GetPlayerAtTable();
        player.IsAtTable = true;
    }

    public void Undo()
    {
        Player player = _gameState.GetPlayerAtTable();
        player.IsAtTable = false;

        _gameState.PlayerAtTableId = _previousPlayerAtTableId;
        player = _gameState.GetPlayerAtTable();
        player.Score = _previousScore;
        player.CurrentBreak = _previousBreak;
        player.HighestBreak = _previousHighestBreak;
        player.AverageBreak = _previousAverageBreak;
        player.BreakSum = _previousBreakSum;
        player.BreakCount = _previousBreakCount;
        player.TotalFouls = _previousTotalFouls;
        player.ConsecutiveFouls = _previousConsecutiveFouls;
        player.IsAtTable = true;
        if (_gameState.GameSettings.ResetRackOnThirdFoul)
        {
            _gameState.BallsOnTable = _previousBallsOnTable;
            _gameState.CurrentRack = _previousRackNumber;
        }

        _gameState.RemoveLastBreakFromHistory();
    }
}
