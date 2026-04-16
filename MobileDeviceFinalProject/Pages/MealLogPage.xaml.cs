using MobileDeviceFinalProject.PageModels;

namespace MobileDeviceFinalProject.Pages
{
    public partial class MealLogPage : ContentPage
    {
        public MealLogPage(MealLogPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}
