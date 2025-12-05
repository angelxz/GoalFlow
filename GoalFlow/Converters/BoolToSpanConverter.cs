using System.Globalization;

namespace GoalFlow.Converters
{
    // Logic: 
    // If Edit Mode (True) -> Span 1 column (because Delete button takes the other slot).
    // If Add Mode (False) -> Span 2 columns (Save button takes full width).
    public class BoolToSpanConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // Check if the value is a boolean.
            if (value is bool isEditMode)
            {
                // If it's true, return 1.
                if (isEditMode)
                {
                    return 1;
                }
                // If it's false, return 2.
                else 
                {
                    return 2;
                }
            }

            // Recommended: If the value is null or not a boolean, 
            // return null or Binding.DoNothing to prevent crashes/unexpected results.
            // Returning 2 here means that if the source property is null or a string, 
            // the target property gets 2, which might be fine for your specific case.
            return 2; 
        }
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}