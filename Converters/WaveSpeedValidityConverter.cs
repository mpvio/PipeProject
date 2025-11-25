using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PipeProject.Converters
{
    public class WaveSpeedValidityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double speed)
            {
                if (speed > 0 && speed <= 1500)
                    return Brushes.LightGreen;   // Valid range
                return Brushes.LightPink;        // Invalid
            }

            if (value is string text)
            {
                // check if empty
                if (string.IsNullOrEmpty(text)) return Brushes.LightPink;
                // Try parsing if it's a string
                if (double.TryParse(text, out double parsedSpeed))
                {
                    if (parsedSpeed > 0 && parsedSpeed <= 1500)
                        return Brushes.LightGreen;
                    return Brushes.LightPink;
                }

            }



            return Brushes.LightPink;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}