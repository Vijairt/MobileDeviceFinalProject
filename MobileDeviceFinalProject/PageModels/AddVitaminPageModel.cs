using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobileDeviceFinalProject.Data;
using MobileDeviceFinalProject.Models;

namespace MobileDeviceFinalProject.PageModels
{
    public partial class AddVitaminPageModel : ObservableObject
    {
        private readonly VitaminRepository _vitaminRepository;
        private readonly ModalErrorHandler _errorHandler;

        [ObservableProperty] private string _name = string.Empty;
        [ObservableProperty] private string _dosage = string.Empty;
        [ObservableProperty] private string _benefits = string.Empty;
        [ObservableProperty] private bool _isDaily = true;
        [ObservableProperty] private bool _isBusy;

        public AddVitaminPageModel(VitaminRepository vitaminRepository, ModalErrorHandler errorHandler)
        {
            _vitaminRepository = vitaminRepository;
            _errorHandler = errorHandler;
        }

        [RelayCommand]
        private async Task Save()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return;

            IsBusy = true;

            try
            {
                var entry = new VitaminEntry
                {
                    Name = Name,
                    Dosage = Dosage,
                    Benefits = Benefits,
                    IsDaily = IsDaily
                };

                await _vitaminRepository.SaveVitaminAsync(entry);
                await Shell.Current.DisplayAlert("Success", "Vitamin saved!", "OK");
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
        private async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
