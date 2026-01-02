using StraightScorer.Core.Models;
using StraightScorer.Core.Services.Interfaces;

namespace StraightScorer.Core.Services.Commands;

internal class MissCommand(GameState _gameService): IUndoRedoCommand
{
    private int _previousPlayerAtTableId;
    private int _nextPlayerAtTableId;
    private int _previousBreak;

    public void Execute()
    {
        // save previous state
        Player player = _gameService.GetPlayerAtTable();
        _previousPlayerAtTableId = _gameService.PlayerAtTableId;
        _nextPlayerAtTableId = _gameService.GetNextPlayerId();
        _previousBreak = player.CurrentBreak;

        // reset break and isAtTable
        player.CurrentBreak = 0;
        player.IsAtTable = false;
        // change reference to next player
        _gameService.PlayerAtTableId = _nextPlayerAtTableId;
        player = _gameService.GetPlayerAtTable();
        player.IsAtTable = true;
    }

    public void Undo()
    {
        Player player = _gameService.GetPlayerAtTable();
        player.IsAtTable = false;
        
        _gameService.PlayerAtTableId = _previousPlayerAtTableId;
        player = _gameService.GetPlayerAtTable();
        player.CurrentBreak = _previousBreak;
        player.IsAtTable = true;
    }
}
