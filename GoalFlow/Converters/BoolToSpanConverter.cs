using System.Globalization;

namespace GoalFlow.Converters
{
    // Logic: 
    // If Edit Mode (True) -> Span 1 column (because Delete button takes the other slot).
    // If Add Mode (False) -> Span 2 columns (Save button takes full width).
    public class BoolToSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isEditMode && isEditMode)
            {
                return 1; 
            }
            return 2; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}