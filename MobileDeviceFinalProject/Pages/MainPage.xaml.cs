using MobileDeviceFinalProject.Models;
using MobileDeviceFinalProject.PageModels;

namespace MobileDeviceFinalProject.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}