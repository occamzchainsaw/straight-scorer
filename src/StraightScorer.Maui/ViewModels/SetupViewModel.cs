using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using StraightScorer.Core.Models;
using StraightScorer.Core.Services;
using StraightScorer.Core.Services.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace StraightScorer.Maui.ViewModels;

public partial class SetupViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;
    private readonly GameState _gameState;
    public SetupViewModel(
        INavigationService navigation,
        GameState gameState)
    {
        _navigationService = navigation;
        _gameState = gameState;

        PlayerSetups.Add(new PlayerSetupDto(() => TargetScore)
            {
                Name = "Player 1",
                HeadStart = 0,
                IsStarting = true,
            });
        PlayerSetups.Add(new PlayerSetupDto(() => TargetScore)
            {
                Name = "Player 2",
                HeadStart = 0,
                IsStarting = false,
            });

        WeakReferenceMessenger.Default.Register<IsStartingPlayerChangedMessage>(this, (r, m) =>
        {
            foreach (var player in PlayerSetups)
            {
                if (player != m.Value)
                    player.IsStarting = false;
            }
        });
        WeakReferenceMessenger.Default.Register<ValidationChangedMessage>(this, (r, m) =>
        {
            StartGameCommand.NotifyCanExecuteChanged();
        });

        ValidateAllProperties();
        StartGameCommand.NotifyCanExecuteChanged();
    }

    public ObservableCollection<PlayerSetupDto> PlayerSetups { get; private set; } = [];
    
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [Range(1,1000)]
    public partial int TargetScore { get; set; } = 100;
    [ObservableProperty] public partial bool IsTargetScoreValid { get; set; } = true;

    partial void OnTargetScoreChanged(int value)
    {
        IsTargetScoreValid = !GetErrors(nameof(TargetScore)).Any();
        foreach (var player in PlayerSetups)
        {
            player.RefreshValidation();
        }
        StartGameCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand(CanExecute = nameof(CanStartGame))]
    async Task StartGame()
    {
        if (_gameState.GameInProgress)
        {
            await _navigationService.NavigateToAsync("game");
            return;
        }

        _gameState.SetupGame(PlayerSetups, TargetScore);

        await _navigationService.NavigateToAsync("game");
    }

    private bool CanStartGame()
    {
        if (HasErrors) return false;

        if (PlayerSetups.Count == 0) return false;

        if (PlayerSetups.All(p => !p.IsStarting)) return false;

        return PlayerSetups.All(p => !p.HasErrors);
    }
}
