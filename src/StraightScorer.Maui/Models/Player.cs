using CommunityToolkit.Mvvm.ComponentModel;

namespace StraightScorer.Maui.Models;

public partial class Player
{
    public string Name { get; set; } = "";

    public int Score { get; set; }

    public int HeadStart { get; set; }

    public int ConsecutiveFouls { get; set; }

    public string ColorString = "#9A82DB";
}

public partial class PlayerSetupDto : ObservableObject
{
    [ObservableProperty]
    private string _name = "";

    [ObservableProperty]
    private int _headStart;
}

public partial class PlayerViewModel(Player _model) : ObservableObject
{
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
    // todo: add property for color
}