using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Interfaces;
using StraightScorer.Maui.Models;
using StraightScorer.Maui.Services;
using StraightScorer.Maui.Views;

namespace StraightScorer.Maui.ViewModels;

public partial class GameSetupViewModel : BaseViewModel
{
    private readonly GameSession _gameSession;
    private readonly IPopupNavigation _popupNavigation;

    public GameSetupViewModel(GameSession gameSession,
        IPopupNavigation popupNavigation)
    {
        _gameSession = gameSession;
        _popupNavigation = popupNavigation;
        SetupPlayer1.IsStarting = true;
        Title = "Setup";
        Subtitle = "Setup your game of straight pool";
    }

    [ObservableProperty]
    private int _targetScore = 100;
    [ObservableProperty]
    private bool _targetScoreInputError;
    [ObservableProperty]
    private PlayerSetupDto? _editingPlayer;
    [ObservableProperty]
    private bool _editHeadStartInputError;

    private PlayerSetupDto? _originalPlayer;

    [ObservableProperty]
    private PlayerSetupDto _setupPlayer1 = new(1);
    [ObservableProperty]
    private PlayerSetupDto _setupPlayer2 = new(2);

    [RelayCommand]
    private void ValidateTargetScore()
    {
        TargetScoreInputError = TargetScore < 1;
    }

    [RelayCommand]
    private void ValidateHeadStart()
    {
        EditHeadStartInputError = EditingPlayer is null || EditingPlayer.HeadStart < 0;
    }

    [RelayCommand]
    private async Task OpenEditPopup(PlayerSetupDto player)
    {
        _originalPlayer = player;
        EditingPlayer = new PlayerSetupDto
        {
            Name = player.Name,
            HeadStart = player.HeadStart,
            IsStarting = player.IsStarting,
        };

        await _popupNavigation.PushAsync(new EditPlayerPopup(this));
    }

    [RelayCommand]
    private async Task SavePlayerEdits()
    {
        if (_originalPlayer is null || EditingPlayer is null)
            return;

        ValidateHeadStart();
        if (EditHeadStartInputError)
            return;

        _originalPlayer.Name = EditingPlayer.Name;
        _originalPlayer.HeadStart = Math.Max(0, Math.Min(EditingPlayer.HeadStart, TargetScore-1));
        _originalPlayer.IsStarting = EditingPlayer.IsStarting;
        if (_originalPlayer.Index == 1 && _originalPlayer.IsStarting)
            SetupPlayer2.IsStarting = false;
        else if (_originalPlayer.Index == 2 && _originalPlayer.IsStarting)
            SetupPlayer1.IsStarting = false;

        if (!SetupPlayer1.IsStarting && !SetupPlayer2.IsStarting)
            SetupPlayer1.IsStarting = true;

        await _popupNavigation.PopAsync();
    }

    [RelayCommand]
    private async Task CancelPlayerEdit()
    {
        await _popupNavigation.PopAsync();
    }

    [RelayCommand]
    private async Task StartGame()
    {
        await _popupNavigation.PopAllAsync();

        ValidateTargetScore();
        if (TargetScoreInputError)
            return;

        _gameSession.TargetScore = TargetScore;

        _gameSession.Player1 = new Player
        {
            Index = SetupPlayer1.Index,
            Name = SetupPlayer1.Name,
            HeadStart = SetupPlayer1.HeadStart,
            Score = Math.Max(0, SetupPlayer1.HeadStart),
            IsAtTable = SetupPlayer1.IsStarting,
        };
        _gameSession.Player2 = new Player
        {
            Index = SetupPlayer2.Index,
            Name = SetupPlayer2.Name,
            HeadStart = SetupPlayer2.HeadStart,
            Score = Math.Max(0, SetupPlayer2.HeadStart),
            IsAtTable = SetupPlayer2.IsStarting,
        };
        if (_gameSession.Player1.IsAtTable)
            _gameSession.PlayerAtTable = _gameSession.Player1;
        else
            _gameSession.PlayerAtTable = _gameSession.Player2;

        await Shell.Current.GoToAsync("//game");
    }
}