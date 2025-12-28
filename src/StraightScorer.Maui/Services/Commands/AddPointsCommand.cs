using StraightScorer.Maui.Services.Interfaces;
using StraightScorer.Maui.ViewModels;

namespace StraightScorer.Maui.Services.Commands;

public class AddPointsCommand : IUndoRedoCommand
{
    private readonly GameViewModel _gameVm;
    private readonly int _pointsToAdd;
    private int _previousBreak;
    private int _previousBallsOnTable;

    public AddPointsCommand(GameViewModel gameVm, int points)
    {
        _gameVm = gameVm;
        _pointsToAdd = points;
    }

    public void Execute()
    {
        _previousBreak = _gameVm.PlayerAtTable.CurrentBreak;
        _previousBallsOnTable = _gameVm.BallsOnTable;
        if (_gameVm.PlayerAtTable.Score + _pointsToAdd >= _gameVm.TargetScore)
        {
            _gameVm.BallsOnTable -= _pointsToAdd;
            _gameVm.PlayerAtTable.Score = _gameVm.TargetScore;
            _gameVm.WinnerPlayer = _gameVm.PlayerAtTable;
            _gameVm.GameFinished = true;
            return;
        }
        _gameVm.PlayerAtTable.CurrentBreak += _pointsToAdd;
        _gameVm.BallsOnTable -= _pointsToAdd;
    }

    public void Undo()
    {
        _gameVm.PlayerAtTable.CurrentBreak = _previousBreak;
        _gameVm.BallsOnTable = _previousBallsOnTable;
    }
}