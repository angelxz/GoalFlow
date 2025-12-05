using GoalFlow.ViewModels;

namespace GoalFlow.Views
{
    public partial class GoalsTodayPage : ContentPage
    {
        public GoalsTodayPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is GoalsTodayViewModel vm)
            {
                await vm.LoadGoals();
            }
        }
    }
}