using System.Globalization;

namespace Egresoss.Converters;

public class PercentageToHeightConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double percentage)
            return percentage * 110; // altura máxima 110dp
        return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}