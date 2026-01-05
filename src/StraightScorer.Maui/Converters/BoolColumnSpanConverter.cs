using System.Globalization;

namespace StraightScorer.Maui.Converters;

public class BoolColumnSpanConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool input)
            return 1;

        var span = input ? 1 : 2;
        return span;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
