using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace PipeProject.Converters
{
    public class FormValidityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // values[0] = Label
            // values[1] = Diameter  
            // values[2] = Length
            // values[3] = WaveSpeed
            // values[4] = YoungsModulus
            // values[5] = PipeType
            // values[6] = ShowYoungsModulus

            if (values.Any(v => v == null || v == Binding.DoNothing))
                return false;

            // Label validation
            string label = values[0] as string;
            if (string.IsNullOrWhiteSpace(label) || label.Length > 16)
                return false;

            // Number validations
            if (!IsValidNumber(values[1] as string, 0.0001, double.MaxValue)) return false; // Diameter
            if (!IsValidNumber(values[2] as string, 0.0001, double.MaxValue)) return false; // Length
            if (!IsValidNumber(values[3] as string, 0.0001, 1500)) return false; // WaveSpeed

            // Young validation
            bool showYoungsModulus = (bool)values[6];
            if (showYoungsModulus && !IsValidNumber(values[4] as string, 0.0001, double.MaxValue))
                return false;

            return true;
        }

        private bool IsValidNumber(string text, double min, double max)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            if (double.TryParse(text, out double result))
            {
                return result >= min && result <= max;
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}