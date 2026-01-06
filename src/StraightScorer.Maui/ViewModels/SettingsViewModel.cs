using CommunityToolkit.Mvvm.ComponentModel;
using StraightScorer.Maui.Services;

namespace StraightScorer.Maui.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    [ObservableProperty]
    public partial SettingsService Settings { get; set; }

    public List<AppTheme> AvailableThemes { get; } = [AppTheme.Unspecified, AppTheme.Light, AppTheme.Dark];

    public SettingsViewModel(SettingsService settings)
    {
        Settings = settings;
    }
}
