using System.Collections;
using System.Globalization;

namespace StraightScorer.Maui.Converters;

public class IsValidationCollectionEmptyConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is IEnumerable collection)
            return !collection.Cast<object>().Any();

        return true;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
