using MobileDeviceFinalProject.PageModels;

namespace MobileDeviceFinalProject.Pages
{
    public partial class WorkoutPage : ContentPage
    {
        private readonly WorkoutPageModel _viewModel;

        public WorkoutPage(WorkoutPageModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            _viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.ForceReload();
        }
    }
}
