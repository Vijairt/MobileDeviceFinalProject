using MobileDeviceFinalProject.PageModels;

namespace MobileDeviceFinalProject.Pages;

public partial class MealLogPage : ContentPage
{
    private readonly MealLogPageModel _viewModel;

    public MealLogPage(MealLogPageModel viewModel)
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
