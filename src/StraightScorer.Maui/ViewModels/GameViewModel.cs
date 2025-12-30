using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Interfaces;
using StraightScorer.Maui.Models;
using StraightScorer.Maui.Services;
using StraightScorer.Maui.Services.Commands;
using StraightScorer.Maui.Services.Interfaces;
using StraightScorer.Maui.Views;

namespace StraightScorer.Maui.ViewModels;

public partial class GameViewModel : BaseViewModel
{
    private readonly GameSession _gameSession;
    private readonly IUndoRedoService _undoRedoService;
    private readonly IPopupNavigation _popupNavigation;

    public GameViewModel(GameSession gameSession,
        IUndoRedoService undoRedoService,
        IPopupNavigation popupNavigation)
    {
        _gameSession = gameSession;
        _undoRedoService = undoRedoService;
        _popupNavigation = popupNavigation;
        Player1 = new PlayerViewModel(_gameSession.Player1);
        Player2 = new PlayerViewModel(_gameSession.Player2);

        if (Player1.IsAtTable)
            PlayerAtTable = Player1;
        else
            PlayerAtTable = Player2;

        BallsOnTable = _gameSession.BallsOnTable;
        TargetScore = _gameSession.TargetScore;

        Title = "Match In Progress";
    }

    [ObservableProperty]
    private PlayerViewModel _player1;
    [ObservableProperty]
    private PlayerViewModel _player2;
    [ObservableProperty]
    private PlayerViewModel _playerAtTable;
    [ObservableProperty]
    private int _pointsToAdd;
    [ObservableProperty]
    private bool _pointsInputError;
    [ObservableProperty]
    private int _ballsOnTable;
    [ObservableProperty]
    private int _targetScore;
    [ObservableProperty]
    private bool _gameFinished;
    [ObservableProperty]
    private PlayerViewModel? _winnerPlayer;

    [RelayCommand]
    private async Task AddPoints(int points)
    {
        if (PointsToAdd < 0)
        {
            PointsInputError = true;
            return;
        }

        PointsInputError = false;
        var cmd = new AddPointsCommand(this, points);
        _undoRedoService.ExecuteAndAdd(cmd);
        await _popupNavigation.PopAllAsync();
    }

    [RelayCommand]
    private async Task ShowAddPointsPopup()
    {
        PointsInputError = false;
        await _popupNavigation.PushAsync(new AddPointsPopup(this));
    }

    [RelayCommand]
    private async Task CancelAddPoints()
    {
        PointsToAdd = 0;
        PointsInputError = false;
        await _popupNavigation.PopAsync();
    }

    [RelayCommand]
    private void Miss()
    {
        MissCommand cmd = new(this);
        _undoRedoService.ExecuteAndAdd(cmd);
    }

    [RelayCommand]
    private void Foul()
    {
        FoulCommand cmd = new(this);
        _undoRedoService.ExecuteAndAdd(cmd);
    }

    [RelayCommand]
    private void UndoLastAction()
    {
        _undoRedoService.Undo();
    }
}