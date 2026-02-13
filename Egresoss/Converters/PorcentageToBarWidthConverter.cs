using System.Globalization;

namespace Egresoss.Converters;

public class PercentageToBarWidthConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double percentage)
        {
            // Ancho máximo de la barra: 150 (ajusta según necesites)
            return Math.Max(0, Math.Min(150, percentage * 150));
        }
        return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
