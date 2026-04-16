using MobileDeviceFinalProject.PageModels;

namespace MobileDeviceFinalProject.Pages
{
    public partial class WorkoutPage : ContentPage
    {
        public WorkoutPage(WorkoutPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}
