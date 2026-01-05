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
    private bool _gameFinished = false;

    public void Execute()
    {
        Player player = _gameState.GetPlayerAtTable();
        // save previous state
        _previousScore = player.Score;
        _previousBreak = player.CurrentBreak;
        _previousFouls = player.ConsecutiveFouls;
        _previousBallsOnTable = _gameState.BallsOnTable;

        // update score and break
        if (player.Score + _pointsToAdd >= _gameState.TargetScore)
        {
            _pointsToAdd = _gameState.TargetScore - player.Score;
            SetBallsLeft();
            player.Score += _pointsToAdd;
            player.CurrentBreak += _pointsToAdd;
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
        _gameState.BallsOnTable = _previousBallsOnTable;
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
