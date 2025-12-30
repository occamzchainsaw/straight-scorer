using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Interfaces;
using StraightScorer.Maui.Models;
using StraightScorer.Maui.Services;

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

        SetupPlayer1.IsStartingChangedEvent += Player1IsStartingChanged;
        SetupPlayer2.IsStartingChangedEvent += Player2IsStartingChanged;
    }

    [ObservableProperty]
    private int _targetScore = 100;
    [ObservableProperty]
    private bool _targetScoreInputError;

    [ObservableProperty]
    private PlayerSetupDto _setupPlayer1 = new(1);
    [ObservableProperty]
    private PlayerSetupDto _setupPlayer2 = new(2);

    private void Player1IsStartingChanged()
    {
        if (SetupPlayer1.IsStarting)
            SetupPlayer2.IsStarting = false;
    }

    private void Player2IsStartingChanged()
    {
        if (SetupPlayer2.IsStarting)
            SetupPlayer1.IsStarting = false;
    }

    [RelayCommand]
    private void ValidateTargetScore()
    {
        TargetScoreInputError = TargetScore < 1;
    }

    [RelayCommand]
    private void ValidateHeadStart(PlayerSetupDto player)
    {
        player.HeadStartInputError = player.HeadStart < 0 || player.HeadStart >= TargetScore;
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