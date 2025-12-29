using CommunityToolkit.Mvvm.ComponentModel;

namespace StraightScorer.Maui.Models;

public partial class Player
{
    public int Index { get; set; }
    public string Name { get; set; } = "";

    public int Score { get; set; }

    public int HeadStart { get; set; }

    public int ConsecutiveFouls { get; set; }

    public bool IsAtTable { get; set; }

    public string ColorString = "#9A82DB";
}

public partial class PlayerSetupDto : ObservableObject
{
    [ObservableProperty]
    private int _index;

    [ObservableProperty]
    private string _name = "";

    [ObservableProperty]
    private int _headStart;

    [ObservableProperty]
    private bool _isStarting;

    public PlayerSetupDto() { }

    public PlayerSetupDto(int index)
    {
        Index = index;
        Name = $"Player {index}";
    }
}

public partial class PlayerViewModel(Player _model) : ObservableObject
{
    public int Index => _model.Index;
    public string Name => _model.Name;
    public int Score
    {
        get => _model.Score;
        set
        {
            _model.Score = value;
            OnPropertyChanged();
        }
    }
    public int CurrentBreak
    {
        get;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }
    public int ConsecutiveFouls
    {
        get => _model.ConsecutiveFouls;
        set
        {
            _model.ConsecutiveFouls = value;
            OnPropertyChanged();
        }
    }
    public bool IsAtTable
    {
        get => _model.IsAtTable;
        set
        {
            _model.IsAtTable = value;
            OnPropertyChanged();
        }
    }
    // todo: add property for color
}