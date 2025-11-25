using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PipeProject.Converters
{
    public class NumberValidityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double number)
            {
                return number > 0 ? Brushes.LightGreen : Brushes.LightPink;
            }

            
            if (value is string text)
            {
                // check if empty
                if (string.IsNullOrEmpty(text)) return Brushes.LightPink;
                // try parsing if it's a string
                if (double.TryParse(text, out double parsedNumber))
                {
                    return parsedNumber > 0 ? Brushes.LightGreen : Brushes.LightPink;
                }

            }

            return Brushes.LightPink;  // Invalid
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}