using StraightScorer.Core.Models;
using StraightScorer.Core.Services.Interfaces;

namespace StraightScorer.Core.Services.Commands;

public class SafeCommand(GameState _gameState) : IUndoRedoCommand
{
    private int _previousPlayerAtTableId;
    private int _nextPlayerAtTableId;
    private int _previousBreak;
    private int _previousFouls;
    private int _previousHighestBreak;
    private float _previousAverageBreak;
    private int _previousBreakSum;
    private int _previousBreakCount;

    public void Execute()
    {
        // save previous state
        Player player = _gameState.GetPlayerAtTable();
        _gameState.AddBreakToHistory(player.Name, player.CurrentBreak, BreakEndAction.Safe);
        _previousPlayerAtTableId = _gameState.PlayerAtTableId;
        _nextPlayerAtTableId = _gameState.GetNextPlayerId();
        _previousBreak = player.CurrentBreak;
        _previousFouls = player.ConsecutiveFouls;
        _previousHighestBreak = player.HighestBreak;
        _previousAverageBreak = player.AverageBreak;
        _previousBreakSum = player.BreakSum;
        _previousBreakCount = player.BreakCount;

        if (player.CurrentBreak > 0)
        {
            player.BreakCount++;
            player.BreakSum += player.CurrentBreak;
            player.AverageBreak = (float)player.BreakSum / (float)player.BreakCount;
            player.HighestBreak = Math.Max(player.CurrentBreak, player.HighestBreak);
        }

        // reset break and isAtTable
        player.CurrentBreak = 0;
        player.ConsecutiveFouls = 0;
        player.IsAtTable = false;
        // change reference to next player
        _gameState.PlayerAtTableId = _nextPlayerAtTableId;
        player = _gameState.GetPlayerAtTable();
        player.IsAtTable = true;
    }

    public void Undo()
    {
        Player player = _gameState.GetPlayerAtTable();
        player.IsAtTable = false;
        
        _gameState.PlayerAtTableId = _previousPlayerAtTableId;
        player = _gameState.GetPlayerAtTable();
        player.CurrentBreak = _previousBreak;
        player.ConsecutiveFouls = _previousFouls;
        player.IsAtTable = true;
        player.BreakCount = _previousBreakCount;
        player.BreakSum = _previousBreakSum;
        player.AverageBreak = _previousAverageBreak;
        player.HighestBreak = _previousHighestBreak;

        _gameState.RemoveLastBreakFromHistory();
    }
}
