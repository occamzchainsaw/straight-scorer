using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StraightScorer.Maui.Models;
using StraightScorer.Maui.Services;

namespace StraightScorer.Maui.ViewModels;

public partial class GameSetupViewModel : BaseViewModel
{
    private static readonly string[] _playerColors =
    [
        "#8e24aa", "#d81b60", "#3949ab", "#e53935", "#5e35b1",
        "#00897b", "#1e88e5", "#00acc1", "#c0ca33", "#7cb342"
    ];

    private readonly GameSession _gameSession;

    public GameSetupViewModel(GameSession gameSession)
    {
        _gameSession = gameSession;
        AddPlayer();
        AddPlayer();
    }

    [ObservableProperty]
    private int _targetScore;
    [ObservableProperty]
    private int _startingPlayerIndex;

    public ObservableCollection<PlayerSetupDto> SetupPlayers { get; } = [];

    [RelayCommand]
    private void AddPlayer()
    {
        if (SetupPlayers.Count < 10)
        {
            SetupPlayers.Add(new PlayerSetupDto { Name = $"Player{SetupPlayers.Count+1}" });
        }
    }

    [RelayCommand]
    private void RemovePlayers(PlayerSetupDto player)
    {
        if (SetupPlayers.Count > 2)
        {
            SetupPlayers.Remove(player);
        }
    }

    [RelayCommand]
    private void StartGame()
    {
        List<Player> players = [.. SetupPlayers
            .Select(p => new Player
            {
                Name = p.Name,
                HeadStart = p.HeadStart,
                Score = Math.Max(0, p.HeadStart)
            })];
        for (int i = 0; i < players.Count; i++)
        {
            players[i].ColorString = _playerColors[i];
        }
        
        _gameSession.Players = players;
        _gameSession.StartingPlayerIndex = StartingPlayerIndex;
        _gameSession.PlayerAtTableIndex = StartingPlayerIndex;
        _gameSession.TargetScore = TargetScore;

        Shell.Current.GoToAsync("//game");
    }
}