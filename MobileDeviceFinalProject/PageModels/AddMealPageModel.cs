using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobileDeviceFinalProject.Data;
using MobileDeviceFinalProject.Models;

namespace MobileDeviceFinalProject.PageModels
{
    public partial class AddMealPageModel : ObservableObject
    {
        private readonly MealRepository _mealRepository;
        private readonly NutritionixService _nutritionixService;
        private readonly ModalErrorHandler _errorHandler;

        [ObservableProperty] private string _searchQuery = string.Empty;
        [ObservableProperty] private List<NutritionResult> _searchResults = [];
        [ObservableProperty] private NutritionResult? _selectedResult;
        [ObservableProperty] private bool _isSearching;
        [ObservableProperty] private bool _showManualEntry;

        [ObservableProperty] private string _manualFoodName = string.Empty;
        [ObservableProperty] private string _manualCalories = string.Empty;
        [ObservableProperty] private string _manualProtein = string.Empty;
        [ObservableProperty] private string _manualCarbs = string.Empty;
        [ObservableProperty] private string _manualFat = string.Empty;
        [ObservableProperty] private string? _photoPath;
        [ObservableProperty] private bool _isBusy;

        public AddMealPageModel(
            MealRepository mealRepository,
            NutritionixService nutritionixService,
            ModalErrorHandler errorHandler)
        {
            _mealRepository = mealRepository;
            _nutritionixService = nutritionixService;
            _errorHandler = errorHandler;
        }

        [RelayCommand]
        private async Task Search()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
                return;

            IsSearching = true;
            SelectedResult = null;

            try
            {
                SearchResults = await _nutritionixService.SearchFoodAsync(SearchQuery);
                ShowManualEntry = SearchResults.Count == 0;
            }
            catch (Exception ex)
            {
                _errorHandler.HandleError(ex);
                ShowManualEntry = true;
            }
            finally
            {
                IsSearching = false;
            }
        }

        [RelayCommand]
        private void SelectResult(NutritionResult result)
        {
            SelectedResult = result;
        }

        [RelayCommand]
        private async Task ConfirmApiEntry()
        {
            if (SelectedResult == null)
                return;

            IsBusy = true;

            try
            {
                var entry = new MealEntry
                {
                    FoodName = SelectedResult.FoodName,
                    Calories = SelectedResult.Calories,
                    ProteinG = SelectedResult.ProteinG,
                    CarbsG = SelectedResult.CarbsG,
                    FatG = SelectedResult.FatG,
                    ServingQty = SelectedResult.ServingQty,
                    ServingUnit = SelectedResult.ServingUnit,
                    LoggedAt = DateTime.Now,
                    PhotoPath = PhotoPath
                };

                await _mealRepository.SaveAsync(entry);
                await Shell.Current.GoToAsync("..");
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

        [RelayCommand]
        private async Task SaveManualEntry()
        {
            if (string.IsNullOrWhiteSpace(ManualFoodName) || string.IsNullOrWhiteSpace(ManualCalories))
                return;

            IsBusy = true;

            try
            {
                var entry = new MealEntry
                {
                    FoodName = ManualFoodName,
                    Calories = double.TryParse(ManualCalories, out var cal) ? cal : 0,
                    ProteinG = double.TryParse(ManualProtein, out var p) ? p : 0,
                    CarbsG = double.TryParse(ManualCarbs, out var c) ? c : 0,
                    FatG = double.TryParse(ManualFat, out var f) ? f : 0,
                    ServingQty = 1,
                    ServingUnit = "serving",
                    LoggedAt = DateTime.Now,
                    PhotoPath = PhotoPath
                };

                await _mealRepository.SaveAsync(entry);
                await Shell.Current.GoToAsync("..");
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

        [RelayCommand]
        private async Task TakePhoto()
        {
            try
            {
                if (!MediaPicker.Default.IsCaptureSupported)
                    return;

                var photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null)
                {
                    var localPath = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);

                    await using var sourceStream = await photo.OpenReadAsync();
                    await using var localFileStream = File.OpenWrite(localPath);
                    await sourceStream.CopyToAsync(localFileStream);

                    PhotoPath = localPath;
                }
            }
            catch (Exception ex)
            {
                _errorHandler.HandleError(ex);
            }
        }

        [RelayCommand]
        private void ToggleManualEntry()
        {
            ShowManualEntry = !ShowManualEntry;
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
