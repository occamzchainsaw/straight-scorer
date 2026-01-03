using CommunityToolkit.Mvvm.ComponentModel;
using StraightScorer.Core.Models;
using StraightScorer.Core.Services.Commands;
using StraightScorer.Core.Services.Interfaces;
using System.Collections.ObjectModel;

namespace StraightScorer.Core.Services;

public partial class GameState(IUndoRedoService _undoRedoService) : ObservableObject
{
    [ObservableProperty] bool _gameInProgress;
    public ObservableCollection<Player> Players { get; } = [];
    [ObservableProperty] int _ballsOnTable = 15;
    [ObservableProperty] int _targetScore = 100;
    [ObservableProperty] int _playerAtTableId;
    [ObservableProperty] int _winningPlayerId = -1;

    public void SetupGame(ICollection<PlayerSetupDto> players, int targetScore)
    {
        TargetScore = targetScore;
        
        Players.Clear();
        int i = 0;
        foreach (PlayerSetupDto player in players)
        {
            Players.Add(new Player()
            {
                Id = i++,
                Name = player.Name,
                Score = player.HeadStart,
                IsAtTable = player.IsStarting,
            });
        }
        PlayerAtTableId = Players.First(p => p.IsAtTable).Id;
        GameInProgress = true;
    }

    public void AddPoints(int points = 1)
    {
        IUndoRedoCommand command = new AddPointsCommand(this, points);
        _undoRedoService.ExecuteAndAdd(command);
    }

    public void Miss()
    {
        IUndoRedoCommand command = new MissCommand(this);
        _undoRedoService.ExecuteAndAdd(command);
    }

    public void Safe()
    {
        IUndoRedoCommand command = new SafeCommand(this);
        _undoRedoService.ExecuteAndAdd(command);
    }

    public void Foul()
    {
        IUndoRedoCommand command = new FoulCommand(this);
        _undoRedoService.ExecuteAndAdd(command);
    }

    public void UndoLastAction()
    {
        if (_undoRedoService.CanUndo)
            _undoRedoService.Undo();
    }

    //public void AddPlayer()
    //{
    //    int newId = 0;
    //    if (Players.Count > 0)
    //        newId = Players.Max(p => p.Id) + 1;

    //    Players.Add(new Player() { Id = newId });
    //}

    public Player GetPlayer(int id)
    {
        return Players.First(p => p.Id == id);
    }

    public Player GetPlayerAtTable()
    {
        return GetPlayer(PlayerAtTableId);
    }

    public int GetNextPlayerId()
    {
        int nextId = PlayerAtTableId + 1;
        if (nextId >= Players.Count)
            nextId = 0;
        return nextId;
    }

    public void EndGame()
    {
        WinningPlayerId = PlayerAtTableId;
        GameInProgress = false;
        // additional logic like saving the results can be added here
    }

    public void UndoEndGame()
    {
        WinningPlayerId = -1;
    }
}
