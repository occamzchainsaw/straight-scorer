using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StraightScorer.Core.Services;

namespace StraightScorer.Maui.ViewModels;

public partial class GameViewModel : BaseViewModel
{
    private readonly GameState _gameState;

    public GameViewModel(GameState gameState)
    {
        _gameState = gameState;
    }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [CustomValidation(typeof(GameViewModel), nameof(ValidatePointsToAdd))]
    public partial int PointsToAdd { get; set; }

    private static ValidationResult? ValidatePointsToAdd(int points, ValidationContext context)
    {
        var instance = (GameViewModel)context.ObjectInstance;
        int max = instance._gameState.TargetScore;

        if (points < 1 || points > max)
            return new ValidationResult("Invalid");

        return ValidationResult.Success;
    }

    [RelayCommand(CanExecute = nameof(CanAddPoints))]
    void AddPoints()
    {
        _gameState.AddPoints(PointsToAdd);
    }

    private bool CanAddPoints()
    {
        return !HasErrors;
    }

    [RelayCommand]
    void AddOnePoint()
    {
        _gameState.AddPoints(1);
    }

    [RelayCommand]
    void Miss()
    {
        _gameState.Miss();
    }

    [RelayCommand]
    void Safe()
    {
        _gameState.Safe();
    }

    [RelayCommand]
    void Foul()
    {
        _gameState.Foul();
    }
}
