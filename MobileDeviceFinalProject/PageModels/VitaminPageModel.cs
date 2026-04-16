using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobileDeviceFinalProject.Models;

namespace MobileDeviceFinalProject.PageModels
{
    public partial class VitaminPageModel : ObservableObject
    {
        private readonly VitaminRepository _vitaminRepository;
        private readonly ModalErrorHandler _errorHandler;

        [ObservableProperty] private List<VitaminViewModel> _vitamins = [];
        [ObservableProperty] private bool _isBusy;

        public VitaminPageModel(VitaminRepository vitaminRepository, ModalErrorHandler errorHandler)
        {
            _vitaminRepository = vitaminRepository;
            _errorHandler = errorHandler;
        }

        [RelayCommand]
        private async Task Appearing() => await LoadVitaminsAsync();

        [RelayCommand]
        private async Task AddVitamin() => await Shell.Current.GoToAsync("addvitamin");

        [RelayCommand]
        private async Task ToggleVitamin(VitaminViewModel vm)
        {
            try
            {
                if (vm.IsTakenToday)
                    await _vitaminRepository.UnlogVitaminAsync(vm.Entry.Id, DateTime.Today);
                else
                    await _vitaminRepository.LogVitaminAsync(vm.Entry.Id, DateTime.Today);
                await LoadVitaminsAsync();
            }
            catch (Exception ex)
            {
                _errorHandler.HandleError(ex);
            }
        }

        [RelayCommand]
        private async Task DeleteVitamin(VitaminViewModel vm)
        {
            try
            {
                await _vitaminRepository.DeleteVitaminAsync(vm.Entry.Id);
                await LoadVitaminsAsync();
            }
            catch (Exception ex)
            {
                _errorHandler.HandleError(ex);
            }
        }

        private async Task LoadVitaminsAsync()
        {
            IsBusy = true;
            try
            {
                var today = DateTime.Today;
                var entries = await _vitaminRepository.GetAllAsync();
                var logs = await _vitaminRepository.GetLogsForDateAsync(today);
                var loggedIds = logs.Select(l => l.VitaminEntryId).ToHashSet();

                var result = new List<VitaminViewModel>();
                foreach (var e in entries)
                {
                    var streak = await _vitaminRepository.GetStreakAsync(e.Id);
                    result.Add(new VitaminViewModel
                    {
                        Entry = e,
                        IsTakenToday = loggedIds.Contains(e.Id),
                        Streak = streak
                    });
                }
                Vitamins = result;
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

    public class VitaminViewModel
    {
        public VitaminEntry Entry { get; set; } = null!;
        public bool IsTakenToday { get; set; }
        public int Streak { get; set; }
        public string StreakText => Streak > 0 ? $"🔥 {Streak}-day streak — keep it up!" : "Start your streak today!";
        public string TakenLabel => IsTakenToday ? "✅ Taken" : "⬜ Mark as Taken";
    }
}
