using System.Windows.Input;
using GoalFlow.Models;
using GoalFlow.Services; // Import Service
using System.Linq; // For LINQ methods

namespace GoalFlow.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private int _totalPoints;
        private int _goalsCompleted;
        private string _dailyQuote = "Loading inspiration...";

        public int TotalPoints
        {
            get => _totalPoints;
            set => SetProperty(ref _totalPoints, value);
        }

        public int GoalsCompleted
        {
            get => _goalsCompleted;
            set => SetProperty(ref _goalsCompleted, value);
        }

        // ... Commands ...
        public ICommand NavigateToAchievementsCommand { get; }
        public ICommand NavigateToGoalsTodayCommand { get; }
        public ICommand NavigateToGoalsCommand { get; }
        public ICommand RefreshStatsCommand { get; } // Helper for OnAppearing

        public MainViewModel()
        {
            Title = "GoalFlow";

            // Initial Load
            LoadStats();

            // Commands
            NavigateToAchievementsCommand = new Command(async () => await Shell.Current.GoToAsync("CategoriesAchievementsPage"));
            NavigateToGoalsTodayCommand = new Command(async () => await Shell.Current.GoToAsync("GoalsTodayPage"));
            NavigateToGoalsCommand = new Command(async () => await Shell.Current.GoToAsync("GoalsPage"));

            RefreshStatsCommand = new Command(LoadStats);

            // Fetch Quote
            Task.Run(async () =>
            {
                DailyQuote = await ApiService.GetDailyQuoteAsync();
            });
        }

        public void LoadStats()
        {
            // Calculate stats from GoalCompletionRecords
            var completions = GoalService.GetAllCompletionsAsync().Result;
            GoalsCompleted = completions.Count;
            TotalPoints = completions.Sum(record => record.Points);
        }

        public string DailyQuote
        {
            get => _dailyQuote;
            set => SetProperty(ref _dailyQuote, value);
        }
    }
}