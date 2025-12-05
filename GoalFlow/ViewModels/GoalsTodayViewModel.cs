using System.Collections.ObjectModel;
using System.Windows.Input;
using GoalFlow.Models;
using GoalFlow.Services;
using System.Globalization;

namespace GoalFlow.ViewModels
{
    public class GoalsTodayViewModel : BaseViewModel
    {
        public ObservableCollection<Goal> TodayGoals { get; set; }
        public ICommand RefreshCommand { get; }
        public ICommand OpenGoalCommand { get; }

        public GoalsTodayViewModel()
        {
            Title = "Today's Focus";
            TodayGoals = new ObservableCollection<Goal>();
            
            RefreshCommand = new Command(async () => await LoadGoals());
            
            // Navigate to Details when a goal is clicked
            OpenGoalCommand = new Command<Goal>(async (goal) => 
            {
                var navigationParameter = new Dictionary<string, object>
                {
                    { "Goal", goal }
                };
                await Shell.Current.GoToAsync("GoalDetailsPage", navigationParameter);
            });
        }

        public async Task LoadGoals()
        {
            var allGoals = await GoalService.GetGoalsAsync();
            var filtered = new List<Goal>();

            DateTime now = DateTime.Now;

            foreach (var goal in allGoals)
            {
                bool shouldShow = false;

                // 1. Logic for Specific Date
                if (goal.Periodicity == "Date")
                {
                    // Show if date is today or passed, AND not yet marked complete
                    if (goal.LastCompletedDate == null && goal.TargetDate.Date <= now.Date)
                    {
                        shouldShow = true;
                    }
                }
                // 2. Logic for Periodic Goals
                else
                {
                    // If never completed, show it
                    if (goal.LastCompletedDate == null)
                    {
                        shouldShow = true;
                    }
                    else
                    {
                        DateTime last = goal.LastCompletedDate.Value;
                        
                        switch (goal.Periodicity)
                        {
                            case "Daily":
                                // Show if last completion was before today
                                shouldShow = last.Date < now.Date;
                                break;
                                
                            case "Weekly":
                                // Get start of current week (assuming Monday start)
                                var culture = CultureInfo.CurrentCulture;
                                var diff = now.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;
                                if (diff < 0) diff += 7;
                                DateTime startOfWeek = now.Date.AddDays(-diff);
                                
                                // Show if last completion was before the start of this week
                                shouldShow = last.Date < startOfWeek;
                                break;
                                
                            case "Monthly":
                                // Show if different month or different year
                                shouldShow = last.Month != now.Month || last.Year != now.Year;
                                break;
                                
                            case "Yearly":
                                // Show if different year
                                shouldShow = last.Year != now.Year;
                                break;
                        }
                    }
                }

                if (shouldShow)
                {
                    filtered.Add(goal);
                }
            }

            TodayGoals.Clear();
            foreach (var g in filtered) TodayGoals.Add(g);
        }
    }
}