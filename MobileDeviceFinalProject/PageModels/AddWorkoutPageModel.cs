using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobileDeviceFinalProject.Data;
using MobileDeviceFinalProject.Models;

namespace MobileDeviceFinalProject.PageModels
{
    public partial class AddWorkoutPageModel : ObservableObject
    {
        private readonly WorkoutRepository _workoutRepository;
        private readonly ModalErrorHandler _errorHandler;

        [ObservableProperty] private string _exerciseType = string.Empty;
        [ObservableProperty] private string _durationMinutes = string.Empty;
        [ObservableProperty] private string _distanceMiles = string.Empty;
        [ObservableProperty] private string _sets = string.Empty;
        [ObservableProperty] private string _reps = string.Empty;
        [ObservableProperty] private string _notes = string.Empty;
        [ObservableProperty] private bool _isBusy;

        public List<string> ExerciseTypes { get; } =
            ["Running", "Walking", "Cycling", "Swimming", "Lifting", "HIIT", "Yoga", "Other"];

        public AddWorkoutPageModel(WorkoutRepository workoutRepository, ModalErrorHandler errorHandler)
        {
            _workoutRepository = workoutRepository;
            _errorHandler = errorHandler;
        }

        [RelayCommand]
        private async Task Save()
        {
            if (string.IsNullOrWhiteSpace(ExerciseType) || string.IsNullOrWhiteSpace(DurationMinutes))
                return;

            IsBusy = true;

            try
            {
                var entry = new WorkoutEntry
                {
                    ExerciseType = ExerciseType,
                    DurationMinutes = int.TryParse(DurationMinutes, out var d) ? d : 0,
                    DistanceMiles = double.TryParse(DistanceMiles, out var dist) ? dist : null,
                    Sets = int.TryParse(Sets, out var s) ? s : null,
                    Reps = int.TryParse(Reps, out var r) ? r : null,
                    Notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes,
                    LoggedAt = DateTime.Now
                };

                await _workoutRepository.SaveAsync(entry);
                await Shell.Current.DisplayAlert("Success", "Workout saved!", "OK");
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
