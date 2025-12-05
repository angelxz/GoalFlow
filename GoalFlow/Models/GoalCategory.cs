using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GoalFlow.Models
{
    public class GoalCategory : INotifyPropertyChanged
    {
        public required string Name { get; set; }
        public required string Icon { get; set; }
        public required string Color { get; set; } // Hex code

        public Microsoft.Maui.Graphics.Color ColorBrush 
        {
            get 
            {
                if (string.IsNullOrEmpty(Color)) return Microsoft.Maui.Graphics.Colors.Gray;
                return Microsoft.Maui.Graphics.Color.FromArgb(Color);
            }
        }

        // Field to track selection state for UI highlighting
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}