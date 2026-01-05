using StraightScorer.Core.Models;
using System.Globalization;

namespace StraightScorer.Maui.Converters;

public class BreakEndActionColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is BreakEndAction action)
        {
            if (Application.Current is null)
                return null;

            if (Application.Current.UserAppTheme == AppTheme.Light)
            {
                return action switch
                {
                    BreakEndAction.Foul => Application.Current.Resources["Red"],
                    BreakEndAction.Safe => Application.Current.Resources["Blue"],
                    BreakEndAction.Miss => Application.Current.Resources["Gray"],
                    BreakEndAction.Win => Application.Current.Resources["Green"],
                    _ => Application.Current.Resources["Gray"],
                };
            }
            else
            {
                return action switch
                {
                    BreakEndAction.Foul => Application.Current.Resources["RedDark"],
                    BreakEndAction.Safe => Application.Current.Resources["BlueDark"],
                    BreakEndAction.Miss => Application.Current.Resources["Gray800"],
                    BreakEndAction.Win => Application.Current.Resources["GreenDark"],
                    _ => Application.Current.Resources["Gray800"],
                };
            }
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
