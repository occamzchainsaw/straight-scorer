using CommunityToolkit.Mvvm.ComponentModel;

namespace StraightScorer.Maui.ViewModels;

public partial class BaseViewModel : ObservableValidator
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    public partial bool IsBusy { get; set; }

    public bool IsNotBusy => !IsBusy;
}