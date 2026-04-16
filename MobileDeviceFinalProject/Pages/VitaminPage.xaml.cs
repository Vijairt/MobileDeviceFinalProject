using MobileDeviceFinalProject.PageModels;

namespace MobileDeviceFinalProject.Pages
{
    public partial class VitaminPage : ContentPage
    {
        public VitaminPage(VitaminPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}
