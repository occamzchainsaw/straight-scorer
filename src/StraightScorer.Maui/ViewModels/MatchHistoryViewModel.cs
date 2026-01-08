using CommunityToolkit.Mvvm.Input;
using StraightScorer.Core.Models;
using StraightScorer.Core.Services.Interfaces;
using System.Collections.ObjectModel;

namespace StraightScorer.Maui.ViewModels;

public partial class MatchHistoryViewModel : BaseViewModel
{
    private readonly IMatchHistoryService _matchHistoryService;

    public MatchHistoryViewModel(IMatchHistoryService matchHistoryService)
    {
        _matchHistoryService = matchHistoryService;
    }

    public ObservableCollection<MatchResult> MatchResults { get; } = [];

    [RelayCommand]
    public async Task LoadHistoryAsync()
    {
        IsBusy = true;

        try
        {
            var history = await _matchHistoryService.GetMatchesAsync();

            MatchResults.Clear();
            foreach (var match in history)
            {
                MatchResults.Add(match);
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task DeleteMatchAsync(MatchResult match)
    {
        if (match == null) return;

        await _matchHistoryService.DeleteMatchResultAsync(match);
        MatchResults.Remove(match);
    }

    [RelayCommand(CanExecute = nameof(CanClear))]
    public async Task ClearHistoryAsync()
    {
        await _matchHistoryService.ClearMatchHistoryAsync();
        MatchResults.Clear();
    }

    private bool CanClear()
    {
        return MatchResults.Count > 0;
    }
}
