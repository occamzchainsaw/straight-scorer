using CommunityToolkit.Mvvm.ComponentModel;

namespace StraightScorer.Core.Models;

public partial class Player : ObservableObject
{
    public int Id { get; set; }

    [ObservableProperty] string _name = "";
    [ObservableProperty] int _score;
    [ObservableProperty] int _currentBreak;
    [ObservableProperty] int _headStart;
    [ObservableProperty] int _consecutiveFouls;
    [ObservableProperty] bool _isAtTable;

    [ObservableProperty] int _highestBreak;
    [ObservableProperty] float _averageBreak;
    [ObservableProperty] int _breakSum;
    [ObservableProperty] int _breakCount;
    [ObservableProperty] int _totalFouls;
}