using System.Collections.ObjectModel;
using System.Windows.Input;
using GoalFlow.Models;
using GoalFlow.Services;

namespace GoalFlow.ViewModels
{
    [QueryProperty(nameof(GoalToEdit), "Goal")]
    public class GoalDetailsViewModel : BaseViewModel
    {
        public required Goal _goalToEdit;
        public required string _target;
        public required string _what;
        private int _howMuch;
        private string _selectedPeriodicity = "Date";
        private DateTime _untilDate = DateTime.Today;
        private string _selectedCategoryName = "Personal";
        
        // Helper properties for binding the single icon in Edit Mode
        private string _selectedCategoryIcon = "💡"; 
        private string _selectedCategoryColor = "#EF6C00";

        private bool _isDateVisible = true;
        private bool _isEditMode;

        // Form Properties
        public string Target { get => _target; set => SetProperty(ref _target, value); }
        public string What { get => _what; set => SetProperty(ref _what, value); }
        public int HowMuch { get => _howMuch; set => SetProperty(ref _howMuch, value); }
        public DateTime TargetDate { get => _untilDate; set => SetProperty(ref _untilDate, value); }
        public string SelectedCategoryName { get => _selectedCategoryName; set => SetProperty(ref _selectedCategoryName, value); }

        private async void CompleteGoal()
        {
            if (!IsEditMode || _goalToEdit == null)
            {
                await Shell.Current.DisplayAlertAsync("Info", "Save the goal first before completing it.", "OK");
                return;
            }

            // 1. Create Record
            var record = new GoalCompletionRecord
            {
                GoalId = _goalToEdit.Id,
                //GoalName = _goalToEdit.Name,
                Points = _goalToEdit.Points,
                DateCompleted = DateTime.Now
            };

            // 2. Save Record & Update Global Points
            await GoalService.AddCompletionRecordAsync(record);

            // 3. Feedback
            await Shell.Current.DisplayAlertAsync("Congratulations!", $"You have received {record.Points} points!", "OK");
        }

        private async void ViewAchievements()
        {
            // Navigate to Achievements Page, passing the GoalId
            await Shell.Current.GoToAsync($"GoalAchievementsPage?GoalId={_goalToEdit.Id}");
        }

        public string SelectedCategoryIcon
        {
            get => _selectedCategoryIcon;
            set => SetProperty(ref _selectedCategoryIcon, value);
        }

        public string SelectedCategoryColor
        {
            get => _selectedCategoryColor;
            set => SetProperty(ref _selectedCategoryColor, value);
        }
        
        public bool IsDateVisible { get => _isDateVisible; set => SetProperty(ref _isDateVisible, value); }
        public bool IsEditMode 
        { 
            get => _isEditMode; 
            set 
            {
                SetProperty(ref _isEditMode, value);
                OnPropertyChanged(nameof(IsAddMode)); // Notify IsAddMode changed too
            }
        }
        public bool IsAddMode => !IsEditMode; // Helper for UI visibility

        // Data Sources
        public ObservableCollection<GoalCategory> CategoryOptions { get; set; }
        public List<string> PeriodicityOptions { get; } = new List<string> { "Daily", "Weekly", "Monthly", "Yearly", "Date" };

        public string SelectedPeriodicity
        {
            get => _selectedPeriodicity;
            set
            {
                if (SetProperty(ref _selectedPeriodicity, value))
                {
                    IsDateVisible = value == "Date";
                }
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand CompleteGoalCommand { get; }
        public ICommand ViewAchievementsCommand { get; }
        public ICommand SelectCategoryCommand { get; }
        public ICommand CancelCommand { get; }

        public GoalDetailsViewModel()
        {
            Title = "Goal Details";
            
            // Initialize Categories
            CategoryOptions = new ObservableCollection<GoalCategory>
            {
                new GoalCategory { Name = "Finance", Icon = "💰", Color = "#2E7D32" },
                new GoalCategory { Name = "Education", Icon = "🎓", Color = "#1565C0" },
                new GoalCategory { Name = "Health", Icon = "🍎", Color = "#C62828" },
                new GoalCategory { Name = "Personal", Icon = "💡", Color = "#EF6C00" }
            };
            
            // Set initial selection
            UpdateSelectionState("Personal");

            SaveCommand = new Command(async () => await Save());
            DeleteCommand = new Command(async () => await Delete());
            CompleteGoalCommand = new Command(CompleteGoal);
            ViewAchievementsCommand = new Command(ViewAchievements);
            
            SelectCategoryCommand = new Command<GoalCategory>((cat) =>
            {
                if (IsEditMode) return; // Should not happen due to UI hiding, but safe guard
                if (cat != null) UpdateSelectionState(cat.Name);
            });
            
            CancelCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
            
            IsEditMode = false;
        }

        private void UpdateSelectionState(string categoryName)
        {
            SelectedCategoryName = categoryName;
            
            // Update the IsSelected flag for UI visuals
            foreach(var c in CategoryOptions) 
            {
                c.IsSelected = (c.Name == categoryName);
                if (c.IsSelected)
                {
                    SelectedCategoryIcon = c.Icon;
                    SelectedCategoryColor = c.Color;
                }
            }
        }

        public Goal GoalToEdit
        {
            get => _goalToEdit;
            set
            {
                _goalToEdit = value;
                if (_goalToEdit != null)
                {
                    Target = _goalToEdit.Name;
                    What = _goalToEdit.Description;
                    HowMuch = _goalToEdit.Points;
                    SelectedPeriodicity = _goalToEdit.Periodicity;
                    TargetDate = _goalToEdit.TargetDate;
                    
                    // Trigger selection update
                    UpdateSelectionState(_goalToEdit.Category);
                    
                    IsEditMode = true; 
                    Title = "Edit Goal";
                }
            }
        }

        private async Task Save()
        {
            if (string.IsNullOrWhiteSpace(Target))
            {
                await Shell.Current.DisplayAlertAsync("Error", "Target Name is required.", "OK");
                return;
            }

            var goalToSave = _goalToEdit
                ?? new Goal
                {
                    Name = Target,
                    Description = What,
                    Points = HowMuch,
                    Periodicity = SelectedPeriodicity,
                    TargetDate = TargetDate,
                    Category = SelectedCategoryName

                };
            //goalToSave.Name = Target;
            //goalToSave.Description = What;
            //goalToSave.Points = HowMuch;
            //goalToSave.Category = SelectedCategoryName;
            //goalToSave.Periodicity = SelectedPeriodicity;
            //goalToSave.TargetDate = TargetDate;

            await GoalService.SaveGoalAsync(goalToSave);
            await Shell.Current.GoToAsync("..");
        }

        private async Task Delete()
        {
            if (_goalToEdit == null) return;
            bool answer = await Shell.Current.DisplayAlertAsync("Delete", "Are you sure?", "Yes", "No");
            if (answer)
            {
                await GoalService.DeleteGoalAsync(_goalToEdit.Id);
                await Shell.Current.GoToAsync("..");
            }
        }
    }
} 