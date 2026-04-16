using MobileDeviceFinalProject.PageModels;

namespace MobileDeviceFinalProject.Pages
{
    public partial class DashboardPage : ContentPage
    {
        public DashboardPage(DashboardPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}
