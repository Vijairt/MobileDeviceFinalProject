using MobileDeviceFinalProject.PageModels;

namespace MobileDeviceFinalProject.Pages
{
    public partial class DashboardPage : ContentPage
    {
        private readonly DashboardPageModel _viewModel;

        public DashboardPage(DashboardPageModel viewModel)
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
