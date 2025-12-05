using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using GoalFlow.Models;
using GoalFlow.Services;

namespace GoalFlow.ViewModels
{
    public class CategoryStat
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public int TotalPoints { get; set; }
        public int CompletedGoals { get; set; }
        public int TotalGoals { get; set; }
        
        public string ProgressText => $"{CompletedGoals}/{TotalGoals} Goals Done";
    }

    public class CategoriesAchievementsViewModel : BaseViewModel
    {
        public ObservableCollection<CategoryStat> Stats { get; set; }
        public ICommand OpenCategoryCommand { get; }

        public CategoriesAchievementsViewModel()
        {
            Title = "My Achievements";
            Stats = new ObservableCollection<CategoryStat>();

            OpenCategoryCommand = new Command<CategoryStat>(async (stat) => 
            {
                if (stat == null) return;
                
                var navParams = new Dictionary<string, object>
                {
                    { "Name", stat.Name },
                    { "Icon", stat.Icon },
                    { "Color", stat.Color }
                };

                await Shell.Current.GoToAsync("CategoryAchievementsPage", navParams);
            });

            Task.Run(LoadStats);
        }

        public async Task LoadStats()
        {
            IsBusy = true;
            
            // We ONLY use GoalService now
            var goals = await GoalService.GetGoalsAsync();
            
            var categories = new List<GoalCategory>
            {
                new GoalCategory { Name = "Finance", Icon = "💰", Color = "#2E7D32" },
                new GoalCategory { Name = "Education", Icon = "🎓", Color = "#1565C0" },
                new GoalCategory { Name = "Health", Icon = "🍎", Color = "#C62828" },
                new GoalCategory { Name = "Personal", Icon = "💡", Color = "#EF6C00" }
            };

            var newStats = new List<CategoryStat>();

            foreach (var cat in categories)
            {
                // Filter goals for this category
                var catGoals = goals.Where(g => g.Category == cat.Name).ToList();

                // 1. Calculate Points from the GoalCompletionRecords
                int points = 0;
                foreach (var g in catGoals)
                {
                    var records = await GoalService.GetCompletionsForGoalAsync(g.Id);
                    points += records.Sum(r => r.Points);
                }


                // 2. Calculate Ratios (Current Period Status)
                    int total = catGoals.Count;
                int completed = 0;

                foreach (var g in catGoals)
                {
                    if (IsGoalCompletedForPeriod(g))
                    {
                        completed++;
                    }
                }

                newStats.Add(new CategoryStat
                {
                    Name = cat.Name,
                    Icon = cat.Icon,
                    Color = cat.Color,
                    TotalPoints = points,
                    CompletedGoals = completed,
                    TotalGoals = total
                });
            }

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Stats.Clear();
                foreach (var s in newStats) Stats.Add(s);
                IsBusy = false;
            });
        }

        private bool IsGoalCompletedForPeriod(Goal g)
        {
            // One-time date goals
            if (g.Periodicity == "Date")
                return g.IsCompleted;

            // Recurring goals - check LastCompletedDate
            if (g.LastCompletedDate == null) return false;

            DateTime last = g.LastCompletedDate.Value;
            DateTime now = DateTime.Now;

            switch (g.Periodicity)
            {
                case "Daily":
                    return last.Date == now.Date;
                case "Weekly":
                    return IsSameWeek(last, now);
                case "Monthly":
                    return last.Month == now.Month && last.Year == now.Year;
                case "Yearly":
                    return last.Year == now.Year;
                default:
                    return false;
            }
        }

        private bool IsSameWeek(DateTime d1, DateTime d2)
        {
            var cal = DateTimeFormatInfo.CurrentInfo.Calendar;
            var d1Week = cal.GetWeekOfYear(d1, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            var d2Week = cal.GetWeekOfYear(d2, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            return d1Week == d2Week && d1.Year == d2.Year;
        }
    }
}