using MobileDeviceFinalProject.PageModels;

namespace MobileDeviceFinalProject.Pages
{
    public partial class AddVitaminPage : ContentPage
    {
        public AddVitaminPage(AddVitaminPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}
