using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobileDeviceFinalProject.Models;

namespace MobileDeviceFinalProject.PageModels
{
    public partial class DashboardPageModel : ObservableObject
    {
        private readonly MealRepository _mealRepository;
        private readonly WorkoutRepository _workoutRepository;
        private readonly VitaminRepository _vitaminRepository;
        private readonly ModalErrorHandler _errorHandler;

        [ObservableProperty] private string _today = DateTime.Now.ToString("dddd, MMM d");
        [ObservableProperty] private double _totalCaloriesToday;
        [ObservableProperty] private double _calorieGoal = 2000;
        [ObservableProperty] private string _calorieStatus = string.Empty;
        [ObservableProperty] private double _totalProteinToday;
        [ObservableProperty] private double _totalCarbsToday;
        [ObservableProperty] private double _totalFatToday;
        [ObservableProperty] private int _workoutsThisWeek;
        [ObservableProperty] private int _totalWorkoutMinutesThisWeek;
        [ObservableProperty] private string _vitaminStreakSummary = string.Empty;
        [ObservableProperty] private bool _isBusy;

        public DashboardPageModel(MealRepository mealRepository, WorkoutRepository workoutRepository,
            VitaminRepository vitaminRepository, ModalErrorHandler errorHandler)
        {
            _mealRepository = mealRepository;
            _workoutRepository = workoutRepository;
            _vitaminRepository = vitaminRepository;
            _errorHandler = errorHandler;
        }

        [RelayCommand]
        private async Task Appearing() => await LoadDashboardAsync();

        [RelayCommand]
        private async Task Refresh() => await LoadDashboardAsync();

        private async Task LoadDashboardAsync()
        {
            IsBusy = true;
            try
            {
                var today = DateTime.Today;

                var meals = await _mealRepository.GetByDateAsync(today);
                TotalCaloriesToday = meals.Sum(m => m.Calories);
                TotalProteinToday = meals.Sum(m => m.ProteinG);
                TotalCarbsToday = meals.Sum(m => m.CarbsG);
                TotalFatToday = meals.Sum(m => m.FatG);

                var diff = TotalCaloriesToday - CalorieGoal;
                CalorieStatus = diff > 0
                    ? $"Over goal by {diff:F0} calories"
                    : diff < 0
                        ? $"{Math.Abs(diff):F0} calories remaining"
                        : "Daily goal met! 🎉";

                var weekStart = today.AddDays(-(int)today.DayOfWeek);
                var workouts = await _workoutRepository.GetByDateRangeAsync(weekStart, today);
                WorkoutsThisWeek = workouts.Count;
                TotalWorkoutMinutesThisWeek = workouts.Sum(w => w.DurationMinutes);

                var vitamins = await _vitaminRepository.GetAllAsync();
                if (vitamins.Count > 0)
                {
                    var streakParts = new List<string>();
                    foreach (var v in vitamins.Take(3))
                    {
                        var streak = await _vitaminRepository.GetStreakAsync(v.Id);
                        if (streak > 0)
                            streakParts.Add($"{v.Name}: {streak}-day streak 🔥");
                    }
                    VitaminStreakSummary = streakParts.Count > 0
                        ? string.Join("\n", streakParts)
                        : "Log your vitamins today to start a streak!";
                }
                else
                {
                    VitaminStreakSummary = "Add vitamins on the Vitamins tab to track your streaks.";
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
