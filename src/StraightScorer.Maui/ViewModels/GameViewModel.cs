using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StraightScorer.Maui.Models;
using StraightScorer.Maui.Services;
using StraightScorer.Maui.Services.Commands;
using StraightScorer.Maui.Services.Interfaces;

namespace StraightScorer.Maui.ViewModels;

public partial class GameViewModel : BaseViewModel
{
    private readonly GameSession _gameSession;
    private readonly IUndoRedoService _undoRedoService;

    public GameViewModel(GameSession gameSession,
        IUndoRedoService undoRedoService)
    {
        _gameSession = gameSession;
        _undoRedoService = undoRedoService;
        Players = [.. _gameSession.Players.Select(p => new PlayerViewModel(p))];
        PlayerAtTableIndex = _gameSession.PlayerAtTableIndex;
        PlayerAtTable = Players[PlayerAtTableIndex];
        BallsOnTable = _gameSession.BallsOnTable;
        TargetScore = _gameSession.TargetScore;
    }

    public ObservableCollection<PlayerViewModel> Players { get; }
    [ObservableProperty]
    private int _playerAtTableIndex;
    [ObservableProperty]
    private PlayerViewModel _playerAtTable;
    [ObservableProperty]
    private int _ballsOnTable;
    [ObservableProperty]
    private int _targetScore;
    [ObservableProperty]
    private bool _gameFinished;
    [ObservableProperty]
    private PlayerViewModel? _winnerPlayer;

    [RelayCommand]
    void AddPoints(int points)
    {
        var cmd = new AddPointsCommand(this, points);
        _undoRedoService.ExecuteAndAdd(cmd);
    }

    [RelayCommand]
    void Miss()
    {
        int nextPlayerIndex = PlayerAtTableIndex == Players.Count-1 ? 0 : PlayerAtTableIndex+1;
        MissCommand cmd = new(this, nextPlayerIndex);
        _undoRedoService.ExecuteAndAdd(cmd);
    }

    [RelayCommand]
    void Foul()
    {
        int nextPlayerIndex = PlayerAtTableIndex == Players.Count-1 ? 0 : PlayerAtTableIndex+1;
        FoulCommand cmd = new(this, nextPlayerIndex);
        _undoRedoService.ExecuteAndAdd(cmd);
    }

    [RelayCommand]
    void UndoLastAction()
    {
        _undoRedoService.Undo();
    }
}