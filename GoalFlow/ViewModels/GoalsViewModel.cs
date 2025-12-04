using System.Collections.ObjectModel;
using System.Windows.Input;
using GoalFlow.Models;
using GoalFlow.Services;

namespace GoalFlow.ViewModels
{
    public class GoalsViewModel : BaseViewModel
    {
        private List<Goal> _allGoals;
        public ObservableCollection<Goal> DisplayedGoals { get; set; }
        public ObservableCollection<GoalCategory> Categories { get; set; }
        
        public ICommand AddGoalCommand { get; }
        public ICommand EditGoalCommand { get; }
        public ICommand FilterCategoryCommand { get; }
        public ICommand ShowAllCommand { get; }

        public GoalsViewModel()
        {
            Title = "My Goals";
            Categories = new ObservableCollection<GoalCategory>
            {
                new GoalCategory { Name = "Finance", Icon = "💰", Color = "#2E7D32" },
                new GoalCategory { Name = "Education", Icon = "🎓", Color = "#1565C0" },
                new GoalCategory { Name = "Health", Icon = "🍎", Color = "#C62828" },
                new GoalCategory { Name = "Personal", Icon = "💡", Color = "#EF6C00" }
            };

            DisplayedGoals = new ObservableCollection<Goal>();
            _allGoals = new List<Goal>();

            AddGoalCommand = new Command(async () => await Shell.Current.GoToAsync("GoalDetailsPage"));
            
            EditGoalCommand = new Command<Goal>(async (goal) => 
            {
                var navigationParameter = new Dictionary<string, object>
                {
                    { "Goal", goal }
                };
                await Shell.Current.GoToAsync("GoalDetailsPage", navigationParameter);
            });

            FilterCategoryCommand = new Command<GoalCategory>((category) => 
            {
                if (category == null) return;
                
                // Update Selection UI
                foreach(var c in Categories) c.IsSelected = false;
                category.IsSelected = true;

                var filtered = _allGoals.Where(g => g.Category == category.Name).ToList();
                UpdateDisplay(filtered);
            });

            ShowAllCommand = new Command(() => 
            {
                // Clear Selection UI
                foreach(var c in Categories) c.IsSelected = false;

                UpdateDisplay(_allGoals);
            });
        }

        public async Task LoadGoals()
        {
            _allGoals = await GoalService.GetGoalsAsync();
            UpdateDisplay(_allGoals);
        }

        private void UpdateDisplay(List<Goal> goals)
        {
            DisplayedGoals.Clear();
            foreach (var goal in goals) DisplayedGoals.Add(goal);
        }
    }
}