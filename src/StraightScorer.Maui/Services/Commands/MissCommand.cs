using StraightScorer.Maui.Models;
using StraightScorer.Maui.Services.Interfaces;
using StraightScorer.Maui.ViewModels;

namespace StraightScorer.Maui.Services.Commands;

public class MissCommand : IUndoRedoCommand
{
    private readonly GameViewModel _gameVm;
    private PlayerViewModel? _player1Previous = null!;
    private PlayerViewModel? _player2Previous = null!;
    private PlayerViewModel? _playerAtTablePrevious = null!;

    public MissCommand(GameViewModel gameVm)
    {
        _gameVm = gameVm;
    }
    
    public void Execute()
    {
        _player1Previous = _gameVm.Player1;
        _player2Previous = _gameVm.Player2;
        _playerAtTablePrevious = _gameVm.PlayerAtTable;

        _gameVm.PlayerAtTable.ConsecutiveFouls = 0;
        _gameVm.PlayerAtTable.CurrentBreak = 0;
        _gameVm.PlayerAtTable.IsAtTable = false;

        _gameVm.PlayerAtTable = _playerAtTablePrevious.Index == 1 ? _gameVm.Player2 : _gameVm.Player1;
        _gameVm.PlayerAtTable.IsAtTable = true;

        //todo: add last break to history
    }

    public void Undo()
    {
        _gameVm.Player1 = _player1Previous ??
            throw new NullReferenceException("Player 1 previous state was not saved");
        _gameVm.Player2 = _player2Previous ??
            throw new NullReferenceException("Player 2 previous state was not saved");
        _gameVm.PlayerAtTable = _playerAtTablePrevious ??
            throw new NullReferenceException("Player at table previous state was not saved");
    }
}