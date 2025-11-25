using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PipeProject.Converters
{
    public class LabelLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                if (string.IsNullOrWhiteSpace(text))
                    return Brushes.LightYellow;  // Empty
                if (text.Length <= 16)
                    return Brushes.LightGreen;   // Valid length
                return Brushes.LightPink;        // Too long
            }
            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}