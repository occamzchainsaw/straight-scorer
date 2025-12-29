using StraightScorer.Maui.Models;
using StraightScorer.Maui.Services.Interfaces;
using StraightScorer.Maui.ViewModels;

namespace StraightScorer.Maui.Services.Commands;

public class AddPointsCommand : IUndoRedoCommand
{
    private readonly GameViewModel _gameVm;
    private int _pointsToAdd;
    private PlayerViewModel? _playerPrevious = null;
    private int _previousBallsOnTable;

    public AddPointsCommand(GameViewModel gameVm, int points)
    {
        _gameVm = gameVm;
        _pointsToAdd = points;
    }

    public void Execute()
    {
        _playerPrevious = _gameVm.PlayerAtTable;
        _previousBallsOnTable = _gameVm.BallsOnTable;
        if (_gameVm.PlayerAtTable.Score + _pointsToAdd >= _gameVm.TargetScore)
        {
            _pointsToAdd = _gameVm.TargetScore - _gameVm.PlayerAtTable.Score;
            CalculateBallsLeft();
            _gameVm.PlayerAtTable.Score = _gameVm.TargetScore;
            _gameVm.WinnerPlayer = _gameVm.PlayerAtTable;
            _gameVm.GameFinished = true;
            return;
        }
        _gameVm.PlayerAtTable.CurrentBreak += _pointsToAdd;
        _gameVm.PlayerAtTable.Score += _pointsToAdd;
        CalculateBallsLeft();
    }

    private void CalculateBallsLeft()
    {
        for (int i = 0; i < _pointsToAdd; i++)
        {
            _gameVm.BallsOnTable--;
            if (_gameVm.BallsOnTable == 1)
                _gameVm.BallsOnTable = 15;
        }
    }

    public void Undo()
    {
        _gameVm.PlayerAtTable = _playerPrevious ?? 
            throw new NullReferenceException("The player's previous state was not saved");
        _gameVm.BallsOnTable = _previousBallsOnTable;
    }
}