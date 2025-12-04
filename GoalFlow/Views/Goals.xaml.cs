using GoalFlow.ViewModels;

namespace GoalFlow.Views
{
    public partial class GoalsPage : ContentPage
    {
        public GoalsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is GoalsViewModel vm)
            {
                await vm.LoadGoals();
            }
        }
    }
}