namespace StraightScorer.Core.Services.Interfaces;

public interface INavigationService
{
    Task NavigateToAsync(string route);
    Task GoBackAsync();
}
