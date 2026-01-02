using StraightScorer.Core.Models;
using StraightScorer.Core.Services.Interfaces;

namespace StraightScorer.Core.Services.Commands;

internal class FoulCommand(GameState _gameService) : IUndoRedoCommand
{
    private int _previousPlayerAtTableId;
    private int _nextPlayerAtTableId;
    private int _previousScore;
    private int _previousBreak;
    private int _previousConsecutiveFouls;

    public void Execute()
    {
        Player player = _gameService.GetPlayerAtTable();
        _previousPlayerAtTableId = _gameService.PlayerAtTableId;
        _nextPlayerAtTableId = _gameService.GetNextPlayerId();
        _previousScore = player.Score;
        _previousBreak = player.CurrentBreak;
        _previousConsecutiveFouls = player.ConsecutiveFouls;

        // foul logic
        if (player.ConsecutiveFouls == 2)
        {
            // third consecutive foul
            player.Score -= 15;
            player.ConsecutiveFouls = 0;
        }
        else
        {
            player.Score--;
            player.CurrentBreak--;
            player.ConsecutiveFouls++;
        }
        player.IsAtTable = false;

        _gameService.PlayerAtTableId = _nextPlayerAtTableId;
        // change reference to next player
        player = _gameService.GetPlayerAtTable();
        player.IsAtTable = true;
    }

    public void Undo()
    {
        Player player = _gameService.GetPlayerAtTable();
        player.IsAtTable = false;

        _gameService.PlayerAtTableId = _previousPlayerAtTableId;
        player = _gameService.GetPlayerAtTable();
        player.Score = _previousScore;
        player.CurrentBreak = _previousBreak;
        player.ConsecutiveFouls = _previousConsecutiveFouls;
        player.IsAtTable = true;
    }
}
