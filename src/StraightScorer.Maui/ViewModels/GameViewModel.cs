using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Interfaces;
using StraightScorer.Core.Models;
using StraightScorer.Core.Services;
using StraightScorer.Core.Services.Interfaces;
using StraightScorer.Maui.Views.Popup;

namespace StraightScorer.Maui.ViewModels;

public partial class GameViewModel : BaseViewModel
{
    private readonly IPopupNavigation _popupNavigation;
    private readonly IMatchHistoryService _matchHistoryService;

    public GameViewModel(
        GameState gameState, 
        IPopupNavigation popupNavigation,
        IMatchHistoryService matchHistoryService)
    {
        CurrentGameState = gameState;
        _popupNavigation = popupNavigation;
        _matchHistoryService = matchHistoryService;

        CurrentGameState.PropertyChanged += async (s, e) =>
        {
            if (e.PropertyName == nameof(CurrentGameState.PlayerAtTableId))
                OnPropertyChanged(nameof(PlayerAtTable));

            if (e.PropertyName == nameof(CurrentGameState.WinningPlayerId))
            {
                WinningPlayer = CurrentGameState.GetPlayer(CurrentGameState.WinningPlayerId);
                if (WinningPlayer is not null)
                {
                    if (_popupNavigation.PopupStack.Any(p => p is EndGamePopup))
                        return;

                    await _popupNavigation.PushAsync(new EndGamePopup(this));
                }
            }

            if (e.PropertyName == nameof(CurrentGameState.GameInProgress))
            {
                OnPropertyChanged(nameof(IsHeadToHead));
                AddPointsCommand.NotifyCanExecuteChanged();
                AddOnePointCommand.NotifyCanExecuteChanged();
                MissCommand.NotifyCanExecuteChanged();
                SafeCommand.NotifyCanExecuteChanged();
                FoulCommand.NotifyCanExecuteChanged();
            }
        };
    }

    [ObservableProperty]
    public partial GameState CurrentGameState { get; set; }

    public bool IsHeadToHead => CurrentGameState.Players.Count == 2;
    public Player PlayerAtTable => CurrentGameState.GetPlayerAtTable();
    public Player? WinningPlayer { get; set; }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [CustomValidation(typeof(GameViewModel), nameof(ValidatePointsToAdd))]
    public partial int PointsToAdd { get; set; }

    private static ValidationResult? ValidatePointsToAdd(int points, ValidationContext context)
    {
        var instance = (GameViewModel)context.ObjectInstance;
        int max = instance.CurrentGameState.TargetScore;

        if (points < 1 || points > max)
            return new ValidationResult("Invalid");

        return ValidationResult.Success;
    }

    [RelayCommand(CanExecute = nameof(CanAddPoints))]
    void AddPoints()
    {
        CurrentGameState.AddPoints(PointsToAdd);
    }

    private bool CanAddPoints()
    {
        return !HasErrors && CurrentGameState.GameInProgress;
    }

    [RelayCommand(CanExecute = nameof(IsGameInProgress))]
    void AddOnePoint()
    {
        CurrentGameState.AddPoints(1);
    }

    [RelayCommand(CanExecute = nameof(IsGameInProgress))]
    void Miss()
    {
        CurrentGameState.Miss();
    }

    [RelayCommand(CanExecute = nameof(IsGameInProgress))]
    void Safe()
    {
        CurrentGameState.Safe();
    }

    [RelayCommand(CanExecute = nameof(IsGameInProgress))]
    void Foul()
    {
        CurrentGameState.Foul();
    }

    private bool IsGameInProgress() => CurrentGameState.GameInProgress;

    [RelayCommand]
    void Undo()
    {
        CurrentGameState.UndoLastAction();
    }

    [RelayCommand]
    async Task CloseEndGamePopup()
    {
        await _popupNavigation.PopAsync();
    }

    [RelayCommand]
    async Task SaveMatchResult()
    {
        MatchResult result = new()
        {
            Players = [.. CurrentGameState.Players.Select(p => new PlayerMatchSummary()
            {
                Name = p.Name,
                FinalScore = p.Score,
                AverageBreak = p.AverageBreak,
                HighestBreak = p.HighestBreak,
                TotalFouls = p.TotalFouls,
            })]
        };
        await _matchHistoryService.SaveMatchResultAsync(result);
        await _popupNavigation.PopAsync();
    }
}
