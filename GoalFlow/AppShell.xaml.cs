using GoalFlow.Views;

namespace GoalFlow
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(GoalsPage), typeof(GoalsPage));
            Routing.RegisterRoute(nameof(GoalDetailsPage), typeof(GoalDetailsPage));
            Routing.RegisterRoute(nameof(GoalAchievementsPage), typeof(GoalAchievementsPage));
        }
    }
}