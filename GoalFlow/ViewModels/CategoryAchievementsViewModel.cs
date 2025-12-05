using System.Collections.ObjectModel;
using System.Windows.Input;
using GoalFlow.Models;
using GoalFlow.Services;

namespace GoalFlow.ViewModels
{
    // Helper class for the list items
    public class AchievedGoalItem
    {
        public required string Name { get; set; }
        public int TimesCompleted { get; set; }
        public DateTime LastDate { get; set; }
        public int TotalPoints { get; set; }
        public required string CategoryColor { get; set; }
    }

    [QueryProperty(nameof(CategoryName), "Name")]
    [QueryProperty(nameof(CategoryIcon), "Icon")]
    [QueryProperty(nameof(CategoryColor), "Color")]
    public class CategoryAchievementsViewModel : BaseViewModel
    {
        public required string _categoryName;
        public required string _categoryIcon;
        public required string _categoryColor;

        public string CategoryName
        {
            get => _categoryName;
            set
            {
                SetProperty(ref _categoryName, value);
                Title = $"What did I achieve with {value}?";
                // Trigger load when category is set
                Task.Run(LoadData);
            }
        }

        public string CategoryIcon { get => _categoryIcon; set => SetProperty(ref _categoryIcon, value); }
        public string CategoryColor { get => _categoryColor; set => SetProperty(ref _categoryColor, value); }

        public ObservableCollection<AchievedGoalItem> AchievedGoals { get; set; }
        public ICommand BackCommand { get; }

        public CategoryAchievementsViewModel()
        {
            AchievedGoals = new ObservableCollection<AchievedGoalItem>();
            BackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        }

        public async Task LoadData()
        {
            if (string.IsNullOrEmpty(CategoryName)) return;

            IsBusy = true;
            var allGoals = await GoalService.GetGoalsAsync();

            // Filter: Matches Category
            var relevantGoals = allGoals
                .Where(g => g.Category == CategoryName)
                .OrderBy(g => g.Name) // Alphabetical order
                .ToList();

            var displayList = new List<AchievedGoalItem>();

            foreach (var goal in relevantGoals)
            {
                var completions = await GoalService.GetCompletionsForGoalAsync(goal.Id);

                if (completions.Count == 0) continue; // Skip if no completions

                displayList.Add(new AchievedGoalItem
                {
                    Name = goal.Name,
                    TimesCompleted = completions.Count,
                    LastDate = completions.Max(c => c.DateCompleted), 
                    TotalPoints = completions.Sum(c => c.Points),
                    CategoryColor = CategoryColor
                });
            }

            MainThread.BeginInvokeOnMainThread(() =>
            {
                AchievedGoals.Clear();
                foreach (var item in displayList) AchievedGoals.Add(item);
                IsBusy = false;
            });
        }
    }
}