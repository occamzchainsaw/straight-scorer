using StraightScorer.Core.Models;
using StraightScorer.Core.Services.Interfaces;

namespace StraightScorer.Core.Services.Commands;

public class SafeCommand(GameState _gameState) : IUndoRedoCommand
{
    private int _previousPlayerAtTableId;
    private int _nextPlayerAtTableId;
    private int _previousBreak;
    private int _previousFouls;

    public void Execute()
    {
        // save previous state
        Player player = _gameState.GetPlayerAtTable();
        _gameState.AddBreakToHistory(player.Name, player.CurrentBreak, BreakEndAction.Safe);
        _previousPlayerAtTableId = _gameState.PlayerAtTableId;
        _nextPlayerAtTableId = _gameState.GetNextPlayerId();
        _previousBreak = player.CurrentBreak;
        _previousFouls = player.ConsecutiveFouls;

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

        _gameState.RemoveLastBreakFromHistory();
    }
}
