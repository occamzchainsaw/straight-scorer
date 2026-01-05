using CommunityToolkit.Mvvm.ComponentModel;

namespace StraightScorer.Core.Models;

public partial class Break : ObservableObject
{
    [ObservableProperty] string _playerName = "";
    [ObservableProperty] int _pointsScored;
    [ObservableProperty] BreakEndAction _endAction;
    [ObservableProperty] bool _isLast;
}

public enum BreakEndAction
{
    Safe,
    Miss,
    Foul,
    Win
}
