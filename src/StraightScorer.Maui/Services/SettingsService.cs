using CommunityToolkit.Mvvm.ComponentModel;
using StraightScorer.Core.Services.Interfaces;

namespace StraightScorer.Maui.Services;

public partial class SettingsService : ObservableObject, IGameSettings
{
    private const string _themeKey = "app_theme";
    private const string _resetRackOnThirdFoulKey = "reset_rack_on_third_foul";

    [ObservableProperty]
    public partial AppTheme CurrentTheme { get; set; }

    [ObservableProperty]
    public partial bool ResetRackOnThirdFoul { get; set; }

    public SettingsService()
    {
        int savedTheme = Preferences.Default.Get(_themeKey, (int)AppTheme.Unspecified);
        CurrentTheme = (AppTheme)savedTheme;
        ResetRackOnThirdFoul = Preferences.Default.Get(_resetRackOnThirdFoulKey, false);
        ApplyTheme(CurrentTheme);
    }

    public void ToggleTheme()
    {
        CurrentTheme = CurrentTheme == AppTheme.Dark ? AppTheme.Light : AppTheme.Dark;
    }

    partial void OnCurrentThemeChanged(AppTheme value)
    {
        Preferences.Default.Set(_themeKey, (int)value);
        ApplyTheme(value);
    }

    partial void OnResetRackOnThirdFoulChanged(bool value)
    {
        Preferences.Default.Set(_resetRackOnThirdFoulKey, value);
    }

    private void ApplyTheme(AppTheme theme)
    {
        if (Application.Current != null)
            Application.Current.UserAppTheme = theme;
    }

    bool IGameSettings.ResetRackOnThirdFoul => ResetRackOnThirdFoul;
}
