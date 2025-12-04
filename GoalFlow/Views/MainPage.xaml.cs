using GoalFlow.ViewModels;

namespace GoalFlow.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Refresh stats when the page comes into view
            if (BindingContext is MainViewModel viewModel)
            {
                viewModel.RefreshStatsCommand.Execute(null);
            }
        }
    }
}