using GoalFlow.ViewModels;

namespace GoalFlow.Views
{
    public partial class CategoriesAchievementsPage : ContentPage
    {
        public CategoriesAchievementsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is CategoriesAchievementsViewModel vm)
            {
                await vm.LoadStats();
            }
        }
    }
}