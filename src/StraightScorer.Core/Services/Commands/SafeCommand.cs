using StraightScorer.Core.Models;
using StraightScorer.Core.Services.Interfaces;

namespace StraightScorer.Core.Services.Commands;

public class SafeCommand(GameState _gameState) : IUndoRedoCommand
{
    private int _previousPlayerAtTableId;
    private int _nextPlayerAtTableId;
    private int _previousBreak;

    public void Execute()
    {
        // save previous state
        Player player = _gameState.GetPlayerAtTable();
        _gameState.BreakHistory.Add(new Break
        {
            PlayerName = player.Name,
            PointsScored = player.CurrentBreak,
            EndAction = BreakEndAction.Safe
        });
        _previousPlayerAtTableId = _gameState.PlayerAtTableId;
        _nextPlayerAtTableId = _gameState.GetNextPlayerId();
        _previousBreak = player.CurrentBreak;

        // reset break and isAtTable
        player.CurrentBreak = 0;
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
        player.IsAtTable = true;

        _gameState.RemoveLastBreakFromHistory();
    }
}
