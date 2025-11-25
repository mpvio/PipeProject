using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PipeProject.Converters
{
    public class TextValidityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                if (string.IsNullOrWhiteSpace(text))
                    return Brushes.LightYellow;  // Empty - warning
                return Brushes.LightGreen;       // Has text - valid
            }
            return Brushes.White;  // Default
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}