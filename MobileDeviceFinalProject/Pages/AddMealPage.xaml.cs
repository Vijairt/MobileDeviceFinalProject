using MobileDeviceFinalProject.PageModels;

namespace MobileDeviceFinalProject.Pages
{
    public partial class AddMealPage : ContentPage
    {
        public AddMealPage(AddMealPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}
