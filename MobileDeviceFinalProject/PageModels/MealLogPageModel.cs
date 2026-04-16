using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobileDeviceFinalProject.Models;

namespace MobileDeviceFinalProject.PageModels
{
    public partial class MealLogPageModel : ObservableObject
    {
        private readonly MealRepository _mealRepository;
        private readonly ModalErrorHandler _errorHandler;
        private DateTime _currentDate = DateTime.Today;

        [ObservableProperty] private List<MealEntry> _meals = [];
        [ObservableProperty] private double _totalCalories;
        [ObservableProperty] private double _totalProtein;
        [ObservableProperty] private double _totalCarbs;
        [ObservableProperty] private double _totalFat;
        [ObservableProperty] private string _selectedDate = DateTime.Today.ToString("MMMM d, yyyy");
        [ObservableProperty] private bool _isBusy;

        public MealLogPageModel(MealRepository mealRepository, ModalErrorHandler errorHandler)
        {
            _mealRepository = mealRepository;
            _errorHandler = errorHandler;
        }

        [RelayCommand]
        private async Task Appearing() => await LoadMealsAsync();

        [RelayCommand]
        private async Task AddMeal() => await Shell.Current.GoToAsync("addmeal");

        [RelayCommand]
        private async Task DeleteMeal(MealEntry meal)
        {
            try
            {
                await _mealRepository.DeleteAsync(meal.Id);
                await LoadMealsAsync();
            }
            catch (Exception ex)
            {
                _errorHandler.HandleError(ex);
            }
        }

        private async Task LoadMealsAsync()
        {
            IsBusy = true;
            try
            {
                Meals = await _mealRepository.GetByDateAsync(_currentDate);
                TotalCalories = Meals.Sum(m => m.Calories);
                TotalProtein = Meals.Sum(m => m.ProteinG);
                TotalCarbs = Meals.Sum(m => m.CarbsG);
                TotalFat = Meals.Sum(m => m.FatG);
            }
            catch (Exception ex)
            {
                _errorHandler.HandleError(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
