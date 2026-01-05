using StraightScorer.Core.Models;
using StraightScorer.Core.Services.Interfaces;

namespace StraightScorer.Core.Services.Commands;

public class FoulCommand(GameState _gameState) : IUndoRedoCommand
{
    private int _previousPlayerAtTableId;
    private int _nextPlayerAtTableId;
    private int _previousScore;
    private int _previousBreak;
    private int _previousConsecutiveFouls;

    public void Execute()
    {
        Player player = _gameState.GetPlayerAtTable();
        _previousPlayerAtTableId = _gameState.PlayerAtTableId;
        _nextPlayerAtTableId = _gameState.GetNextPlayerId();
        _previousScore = player.Score;
        _previousBreak = player.CurrentBreak;
        _previousConsecutiveFouls = player.ConsecutiveFouls;

        // foul logic
        if (player.ConsecutiveFouls == 2)
        {
            // third consecutive foul
            player.Score -= 15;
            player.ConsecutiveFouls = 0;
            _gameState.AddBreakToHistory(player.Name, -15, BreakEndAction.Foul);
        }
        else
        {
            player.Score--;
            player.ConsecutiveFouls++;
            _gameState.AddBreakToHistory(player.Name, player.CurrentBreak - 1, BreakEndAction.Foul);
        }
        player.CurrentBreak = 0;
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
        player.ConsecutiveFouls = _previousConsecutiveFouls;
        player.IsAtTable = true;

        _gameState.RemoveLastBreakFromHistory();
    }
}
