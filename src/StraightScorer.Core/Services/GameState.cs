using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using StraightScorer.Core.Models;
using StraightScorer.Core.Services.Commands;
using StraightScorer.Core.Services.Interfaces;
using System.Collections.ObjectModel;

namespace StraightScorer.Core.Services;

public partial class GameState(IUndoRedoService _undoRedoService, IGameSettings _gameSettings) : ObservableObject
{
    [ObservableProperty] bool _gameInProgress;
    public ObservableCollection<Player> Players { get; } = [];
    public ObservableCollection<Break> BreakHistory { get; } = [];
    [ObservableProperty] int _ballsOnTable = 15;
    [ObservableProperty] int _currentRack = 1;
    [ObservableProperty] int _targetScore = 100;
    [ObservableProperty] int _playerAtTableId;
    [ObservableProperty] int _winningPlayerId = -1;

    public IGameSettings GameSettings => _gameSettings;

    public void SetupGame(ICollection<PlayerSetupDto> players, int targetScore)
    {
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
        TargetScore = targetScore;
        PlayerAtTableId = Players.First(p => p.IsAtTable).Id;
        BallsOnTable = 15;
        CurrentRack = 1;
        BreakHistory.Clear();
        GameInProgress = true;
    }

    partial void OnGameInProgressChanged(bool value)
    {
        WeakReferenceMessenger.Default.Send(new GameInProgressChangedMessage());
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

    public Player? GetPlayer(int id)
    {
        return Players.FirstOrDefault(p => p.Id == id);
    }

    public Player GetPlayerAtTable()
    {
        return GetPlayer(PlayerAtTableId) ?? Players[0];
    }

    public int GetNextPlayerId()
    {
        int nextId = PlayerAtTableId + 1;
        if (nextId >= Players.Count)
            nextId = 0;
        return nextId;
    }

    public void AddBreakToHistory(string playerName, int pointsScored, BreakEndAction endAction)
    {
        foreach (Break b in BreakHistory)
            b.IsLast = false;

        BreakHistory.Add(new Break()
        {
            PlayerName = playerName,
            PointsScored = pointsScored,
            EndAction = endAction,
            IsLast = true,
        });
    }

    public void RemoveLastBreakFromHistory()
    {
        BreakHistory.RemoveAt(BreakHistory.Count - 1);
        BreakHistory.Last().IsLast = true;
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
        GameInProgress = true;
    }
}

public record GameInProgressChangedMessage();