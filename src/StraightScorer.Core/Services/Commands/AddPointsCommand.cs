using StraightScorer.Core.Models;
using StraightScorer.Core.Services.Interfaces;

namespace StraightScorer.Core.Services.Commands;

public class AddPointsCommand(
    GameState _gameState,
    int _pointsToAdd) : IUndoRedoCommand
{
    private int _previousScore;
    private int _previousBreak;
    private int _previousFouls;
    private int _previousBallsOnTable;
    private int _previousRackNumber;
    private bool _gameFinished = false;

    private int _previousHighestBreak;
    private float _previousAverageBreak;
    private int _previousBreakSum;
    private int _previousBreakCount;

    public void Execute()
    {
        Player player = _gameState.GetPlayerAtTable();
        // save previous state
        _previousScore = player.Score;
        _previousBreak = player.CurrentBreak;
        _previousFouls = player.ConsecutiveFouls;
        _previousBallsOnTable = _gameState.BallsOnTable;
        _previousRackNumber = _gameState.CurrentRack;

        _previousHighestBreak = player.HighestBreak;
        _previousAverageBreak = player.AverageBreak;
        _previousBreakSum = player.BreakSum;
        _previousBreakCount = player.BreakCount;

        // update score and break
        if (player.Score + _pointsToAdd >= _gameState.TargetScore)
        {
            _pointsToAdd = _gameState.TargetScore - player.Score;
            SetBallsLeft();
            player.Score += _pointsToAdd;
            player.CurrentBreak += _pointsToAdd;

            player.BreakCount++;
            player.BreakSum += player.CurrentBreak;
            player.AverageBreak = (float)player.BreakSum / (float)player.BreakCount;
            player.HighestBreak = Math.Max(player.CurrentBreak, player.HighestBreak);

            _gameFinished = true;
            _gameState.EndGame();
            _gameState.AddBreakToHistory(player.Name, player.CurrentBreak, BreakEndAction.Win);
            return;
        }
        player.Score += _pointsToAdd;
        player.CurrentBreak += _pointsToAdd;
        player.ConsecutiveFouls = 0;
        SetBallsLeft();
    }

    public void Undo()
    {
        Player player = _gameState.GetPlayerAtTable();
        player.Score = _previousScore;
        player.CurrentBreak = _previousBreak;
        player.ConsecutiveFouls = _previousFouls;

        player.BreakCount = _previousBreakCount;
        player.BreakSum = _previousBreakSum;
        player.AverageBreak = _previousAverageBreak;
        player.HighestBreak = _previousHighestBreak;

        _gameState.BallsOnTable = _previousBallsOnTable;
        _gameState.CurrentRack = _previousRackNumber;
        if (_gameFinished)
        {
            _gameState.RemoveLastBreakFromHistory();
            _gameState.UndoEndGame();
        }
    }

    private void SetBallsLeft()
    {
        for (int i = 0; i < _pointsToAdd; i++)
        {
            _gameState.BallsOnTable--;
            if (_gameState.BallsOnTable == 1)
            {
                _gameState.BallsOnTable = 15;
                _gameState.CurrentRack++;
            }
        }
    }
}
