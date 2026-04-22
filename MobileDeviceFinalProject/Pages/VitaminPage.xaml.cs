using MobileDeviceFinalProject.PageModels;

namespace MobileDeviceFinalProject.Pages
{
    public partial class VitaminPage : ContentPage
    {
        private readonly VitaminPageModel _viewModel;

        public VitaminPage(VitaminPageModel viewModel)
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
