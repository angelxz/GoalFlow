using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GoalFlow.Models
{
    public class GoalCategory : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; } // Hex code

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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}