using StraightScorer.Core.Models;
using StraightScorer.Core.Services.Interfaces;

namespace StraightScorer.Core.Services.Commands;

internal class AddPointsCommand(
    GameState _gameService,
    int _pointsToAdd) : IUndoRedoCommand
{
    private int _previousScore;
    private int _previousBreak;
    private int _previousBallsOnTable;
    private bool _gameFinished = false;

    public void Execute()
    {
        Player player = _gameService.GetPlayerAtTable();
        // save previous state
        _previousScore = player.Score;
        _previousBreak = player.CurrentBreak;
        _previousBallsOnTable = _gameService.BallsOnTable;

        // update score and break
        if (player.Score + _pointsToAdd >= _gameService.TargetScore)
        {
            _pointsToAdd = _gameService.TargetScore - player.Score;
            SetBallsLeft();
            player.Score += _pointsToAdd;
            player.CurrentBreak += _pointsToAdd;
            _gameFinished = true;
            _gameService.EndGame();
            return;
        }
        player.Score += _pointsToAdd;
        player.CurrentBreak += _pointsToAdd;
        SetBallsLeft();
    }

    public void Undo()
    {
        Player player = _gameService.GetPlayerAtTable();
        player.Score = _previousScore;
        player.CurrentBreak = _previousBreak;
        _gameService.BallsOnTable = _previousBallsOnTable;
        if (_gameFinished)
        {
            _gameService.UndoEndGame();
        }
    }

    private void SetBallsLeft()
    {
        for (int i = 0; i < _pointsToAdd; i++)
        {
            _gameService.BallsOnTable--;
            if (_gameService.BallsOnTable == 1)
                _gameService.BallsOnTable = 15;
        }
    }
}
