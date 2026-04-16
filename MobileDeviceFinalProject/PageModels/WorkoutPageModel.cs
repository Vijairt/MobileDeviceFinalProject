using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobileDeviceFinalProject.Models;

namespace MobileDeviceFinalProject.PageModels
{
    public partial class WorkoutPageModel : ObservableObject
    {
        private readonly WorkoutRepository _workoutRepository;
        private readonly ModalErrorHandler _errorHandler;

        [ObservableProperty] private List<WorkoutEntry> _weeklyWorkouts = [];
        [ObservableProperty] private int _totalMinutesThisWeek;
        [ObservableProperty] private double _totalMilesThisWeek;
        [ObservableProperty] private string _progressionTip = string.Empty;
        [ObservableProperty] private bool _isBusy;

        public WorkoutPageModel(WorkoutRepository workoutRepository, ModalErrorHandler errorHandler)
        {
            _workoutRepository = workoutRepository;
            _errorHandler = errorHandler;
        }

        [RelayCommand]
        private async Task Appearing() => await LoadWorkoutsAsync();

        [RelayCommand]
        private async Task AddWorkout() => await Shell.Current.GoToAsync("addworkout");

        [RelayCommand]
        private async Task DeleteWorkout(WorkoutEntry workout)
        {
            try
            {
                await _workoutRepository.DeleteAsync(workout.Id);
                await LoadWorkoutsAsync();
            }
            catch (Exception ex)
            {
                _errorHandler.HandleError(ex);
            }
        }

        private async Task LoadWorkoutsAsync()
        {
            IsBusy = true;
            try
            {
                var today = DateTime.Today;
                var weekStart = today.AddDays(-(int)today.DayOfWeek);
                WeeklyWorkouts = await _workoutRepository.GetByDateRangeAsync(weekStart, today);
                TotalMinutesThisWeek = WeeklyWorkouts.Sum(w => w.DurationMinutes);
                TotalMilesThisWeek = WeeklyWorkouts
                    .Where(w => w.DistanceMiles.HasValue)
                    .Sum(w => w.DistanceMiles!.Value);

                var lastWeekStart = weekStart.AddDays(-7);
                var lastWeekEnd = weekStart.AddDays(-1);
                var lastWeek = await _workoutRepository.GetByDateRangeAsync(lastWeekStart, lastWeekEnd);
                var lastWeekMiles = lastWeek
                    .Where(w => w.DistanceMiles.HasValue)
                    .Sum(w => w.DistanceMiles!.Value);

                if (lastWeekMiles > 0)
                {
                    var goalMiles = Math.Round(Math.Min(lastWeekMiles * 1.10, lastWeekMiles * 1.20), 1);
                    ProgressionTip = $"Great job! Try {goalMiles} miles this week (last week: {lastWeekMiles:F1} mi) 💪";
                }
                else
                {
                    ProgressionTip = "Start logging workouts to get personalized progression tips!";
                }
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
