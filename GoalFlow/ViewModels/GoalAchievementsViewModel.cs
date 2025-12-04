using System.Collections.ObjectModel;
using System.Windows.Input;
using GoalFlow.Models;
using GoalFlow.Services;

namespace GoalFlow.ViewModels
{
    [QueryProperty(nameof(GoalId), "GoalId")]
    public class GoalAchievementsViewModel : BaseViewModel
    {
        private string _goalId;
        private Goal _linkedGoal;
        private int _totalPointsEarned;
        private int _completionCount;
        private string _categoryIcon = "❓";
        private string _goalName;
        private string _categoryColor;

        public string GoalId
        {
            get => _goalId;
            set
            {
                _goalId = value;
                LoadData();
            }
        }

        // UI Properties
        public string GoalName { get => _goalName; set => SetProperty(ref _goalName, value); }
        public string CategoryIcon { get => _categoryIcon; set => SetProperty(ref _categoryIcon, value); }
        public string CategoryColor { get => _categoryColor; set => SetProperty(ref _categoryColor, value); }
        public int TotalPointsEarned { get => _totalPointsEarned; set => SetProperty(ref _totalPointsEarned, value); }
        public int CompletionCount { get => _completionCount; set => SetProperty(ref _completionCount, value); }
        
        public ObservableCollection<GoalCompletionRecord> Records { get; set; }
        public ICommand CloseCommand { get; }

        public GoalAchievementsViewModel()
        {
            Title = "Achievements";
            Records = new ObservableCollection<GoalCompletionRecord>();
            CloseCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        }

        private async void LoadData()
        {
            // 1. Get Goal Details to set Header info
            var goals = await GoalService.GetGoalsAsync();
            _linkedGoal = goals.FirstOrDefault(g => g.Id == GoalId);

            if (_linkedGoal != null)
            {
                GoalName = _linkedGoal.Name;
                CategoryIcon = _linkedGoal.CategoryIcon;
                CategoryColor = _linkedGoal.CategoryColor;
            }

            // 2. Get Completion Records
            var records = await GoalService.GetCompletionsForGoalAsync(GoalId);
            
            Records.Clear();
            int pointsSum = 0;
            foreach (var r in records)
            {
                Records.Add(r);
                pointsSum += r.Points;
            }

            TotalPointsEarned = pointsSum;
            CompletionCount = records.Count;
        }
    }
}