using MobileDeviceFinalProject.PageModels;

namespace MobileDeviceFinalProject.Pages
{
    public partial class AddWorkoutPage : ContentPage
    {
        public AddWorkoutPage(AddWorkoutPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}
