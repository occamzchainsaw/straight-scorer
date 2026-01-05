using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StraightScorer.Core.Models;
using StraightScorer.Core.Services;

namespace StraightScorer.Maui.ViewModels;

public partial class GameViewModel : BaseViewModel
{
    public GameViewModel(GameState gameState)
    {
        CurrentGameState = gameState;

        CurrentGameState.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(CurrentGameState.PlayerAtTableId))
                OnPropertyChanged(nameof(PlayerAtTable));
        };
    }

    [ObservableProperty]
    public partial GameState CurrentGameState { get; set; }

    public bool IsHeadToHead => CurrentGameState.Players.Count == 2;
    public Player PlayerAtTable => CurrentGameState.GetPlayerAtTable();

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
        return !HasErrors;
    }

    [RelayCommand]
    void AddOnePoint()
    {
        CurrentGameState.AddPoints(1);
    }

    [RelayCommand]
    void Miss()
    {
        CurrentGameState.Miss();
    }

    [RelayCommand]
    void Safe()
    {
        CurrentGameState.Safe();
    }

    [RelayCommand]
    void Foul()
    {
        CurrentGameState.Foul();
    }

    [RelayCommand]
    void Undo()
    {
        CurrentGameState.UndoLastAction();
    }
}
