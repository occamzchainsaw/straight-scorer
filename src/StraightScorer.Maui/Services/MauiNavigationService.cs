using StraightScorer.Maui.Services.Interfaces;

namespace StraightScorer.Maui.Services;

public class MauiNavigationService : INavigationService
{
    public Task NavigateToAsync(string route)
    {
        return Shell.Current.GoToAsync(route);
    }

    public Task GoBackAsync()
    {
        return Shell.Current.GoToAsync("..");
    }
}
