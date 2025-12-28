using StraightScorer.Maui.Models;
using StraightScorer.Maui.Services.Interfaces;
using StraightScorer.Maui.ViewModels;

namespace StraightScorer.Maui.Services.Commands;

public class FoulCommand : IUndoRedoCommand
{
    private readonly GameViewModel _gameVm;
    private readonly int _nextPlayerIndex;
    private PlayerViewModel _previousPlayerVm = null!;
    private int _previousPlayerIndex;

    public FoulCommand(GameViewModel gameVm, int nextPlayerIndex)
    {
        _gameVm = gameVm;
        _nextPlayerIndex = nextPlayerIndex;
    }

    public void Execute()
    {
        _previousPlayerIndex = _gameVm.PlayerAtTableIndex;
        _previousPlayerVm = _gameVm.PlayerAtTable;
        ResetPlayerBreak();
        _gameVm.PlayerAtTableIndex = _nextPlayerIndex;
        _gameVm.PlayerAtTable = _gameVm.Players[_nextPlayerIndex];
    }

    public void Undo()
    {
        _gameVm.PlayerAtTableIndex = _previousPlayerIndex;
        _gameVm.PlayerAtTable = _previousPlayerVm;
    }

    private void ResetPlayerBreak()
    {
        _gameVm.PlayerAtTable.ConsecutiveFouls += 1;
        _gameVm.PlayerAtTable.CurrentBreak -= 1;
        _gameVm.PlayerAtTable.Score = _gameVm.PlayerAtTable.CurrentBreak;
    }
}