using SQLite;
using MobileDeviceFinalProject.Models;

namespace MobileDeviceFinalProject.Services;

public class SQLiteService
{
    private SQLiteAsyncConnection _db;

    public async Task Init()
    {
        if (_db != null) return;

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "fittrack.db");
        _db = new SQLiteAsyncConnection(dbPath);

        await _db.CreateTableAsync<MealEntry>();
        await _db.CreateTableAsync<WorkoutEntry>();
        await _db.CreateTableAsync<VitaminEntry>();
    }

    
    // MEALS
    

    public Task<int> AddMeal(MealEntry meal)
    {
        return _db.InsertAsync(meal);
    }

    public Task<List<MealEntry>> GetMeals()
    {
        return _db.Table<MealEntry>().ToListAsync();
    }

    public async Task<List<MealEntry>> GetMealsByDate(DateTime date)
    {
        return await _db.Table<MealEntry>()
            .Where(m => m.LoggedAt.Date == date.Date)
            .ToListAsync();
    }

    // DAILY CALORIES
    public async Task<double> GetDailyCalories(DateTime date)
    {
        var meals = await GetMealsByDate(date);
        return meals.Sum(m => m.Calories);
    }

    // DAILY MACROS
    public async Task<(double protein, double carbs, double fat)> GetDailyMacros(DateTime date)
    {
        var meals = await GetMealsByDate(date);

        return (
            meals.Sum(m => m.ProteinG),
            meals.Sum(m => m.CarbsG),
            meals.Sum(m => m.FatG)
        );
    }

    // OFFLINE MANUAL ENTRY
    public async Task<int> AddManualMeal(string name, int calories, double protein, double carbs, double fat)
    {
        var meal = new MealEntry
        {
            FoodName = name,
            Calories = calories,
            ProteinG = protein,
            CarbsG = carbs,
            FatG = fat,
            LoggedAt = DateTime.Now
        };

        return await _db.InsertAsync(meal);
    }

    // DELETE 
    public Task<int> DeleteMeal(MealEntry meal)
    {
        return _db.DeleteAsync(meal);
    }

    public Task<int> ClearMeals()
    {
        return _db.DeleteAllAsync<MealEntry>();
    }

    
    // WORKOUTS
    

    public Task<int> AddWorkout(WorkoutEntry workout)
    {
        return _db.InsertAsync(workout);
    }

    public Task<List<WorkoutEntry>> GetWorkouts()
    {
        return _db.Table<WorkoutEntry>().ToListAsync();
    }

    
    // VITAMINS
    

    public Task<int> AddVitamin(VitaminEntry vitamin)
    {
        return _db.InsertAsync(vitamin);
    }

    public Task<List<VitaminEntry>> GetVitamins()
    {
        return _db.Table<VitaminEntry>().ToListAsync();
    }
}
