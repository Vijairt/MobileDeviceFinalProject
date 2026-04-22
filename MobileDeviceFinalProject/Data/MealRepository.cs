using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using MobileDeviceFinalProject.Models;

namespace MobileDeviceFinalProject.Data
{
    public class MealRepository
    {
        private bool _hasBeenInitialized;
        private readonly ILogger<MealRepository> _logger;

        public MealRepository(ILogger<MealRepository> logger)
        {
            _logger = logger;
        }

        private async Task Init()
        {
            if (_hasBeenInitialized) return;

            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            try
            {
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS MealEntry (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        FoodName TEXT NOT NULL,
                        Calories REAL NOT NULL,
                        ProteinG REAL NOT NULL,
                        CarbsG REAL NOT NULL,
                        FatG REAL NOT NULL,
                        ServingQty REAL NOT NULL,
                        ServingUnit TEXT NOT NULL,
                        LoggedAt TEXT NOT NULL,
                        PhotoPath TEXT
                    );";
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating MealEntry table");
                throw;
            }

            _hasBeenInitialized = true;
        }

        public async Task SaveAsync(MealEntry entry)
        {
            await Init();

            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO MealEntry
                (FoodName, Calories, ProteinG, CarbsG, FatG, ServingQty, ServingUnit, LoggedAt, PhotoPath)
                VALUES
                (@FoodName, @Calories, @ProteinG, @CarbsG, @FatG, @ServingQty, @ServingUnit, @LoggedAt, @PhotoPath);";

            cmd.Parameters.AddWithValue("@FoodName", entry.FoodName);
            cmd.Parameters.AddWithValue("@Calories", entry.Calories);
            cmd.Parameters.AddWithValue("@ProteinG", entry.ProteinG);
            cmd.Parameters.AddWithValue("@CarbsG", entry.CarbsG);
            cmd.Parameters.AddWithValue("@FatG", entry.FatG);
            cmd.Parameters.AddWithValue("@ServingQty", entry.ServingQty);
            cmd.Parameters.AddWithValue("@ServingUnit", entry.ServingUnit);
            cmd.Parameters.AddWithValue("@LoggedAt", entry.LoggedAt.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@PhotoPath", (object?)entry.PhotoPath ?? DBNull.Value);

            var rows = await cmd.ExecuteNonQueryAsync();
            _logger.LogInformation("Saved meal entry. Rows affected: {Rows}", rows);
        }

        public async Task<List<MealEntry>> GetAllAsync()
        {
            await Init();

            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                SELECT Id, FoodName, Calories, ProteinG, CarbsG, FatG, ServingQty, ServingUnit, LoggedAt, PhotoPath
                FROM MealEntry
                ORDER BY LoggedAt DESC;";

            var meals = new List<MealEntry>();

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                meals.Add(new MealEntry
                {
                    Id = reader.GetInt32(0),
                    FoodName = reader.GetString(1),
                    Calories = reader.GetDouble(2),
                    ProteinG = reader.GetDouble(3),
                    CarbsG = reader.GetDouble(4),
                    FatG = reader.GetDouble(5),
                    ServingQty = reader.GetDouble(6),
                    ServingUnit = reader.GetString(7),
                    LoggedAt = DateTime.Parse(reader.GetString(8)),
                    PhotoPath = reader.IsDBNull(9) ? null : reader.GetString(9)
                });
            }

            return meals;
        }

        public async Task<List<MealEntry>> GetByDateAsync(DateTime date)
        {
            await Init();

            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                SELECT Id, FoodName, Calories, ProteinG, CarbsG, FatG, ServingQty, ServingUnit, LoggedAt, PhotoPath
                FROM MealEntry
                WHERE date(LoggedAt) = date(@date)
                ORDER BY LoggedAt DESC;";
            cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));

            var meals = new List<MealEntry>();

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                meals.Add(new MealEntry
                {
                    Id = reader.GetInt32(0),
                    FoodName = reader.GetString(1),
                    Calories = reader.GetDouble(2),
                    ProteinG = reader.GetDouble(3),
                    CarbsG = reader.GetDouble(4),
                    FatG = reader.GetDouble(5),
                    ServingQty = reader.GetDouble(6),
                    ServingUnit = reader.GetString(7),
                    LoggedAt = DateTime.Parse(reader.GetString(8)),
                    PhotoPath = reader.IsDBNull(9) ? null : reader.GetString(9)
                });
            }

            return meals;
        }

        public async Task<double> GetDailyCaloriesAsync(DateTime date)
        {
            var meals = await GetByDateAsync(date);
            return meals.Sum(m => m.Calories);
        }

        public async Task<(double protein, double carbs, double fat)> GetDailyMacrosAsync(DateTime date)
        {
            var meals = await GetByDateAsync(date);
            return (
                meals.Sum(m => m.ProteinG),
                meals.Sum(m => m.CarbsG),
                meals.Sum(m => m.FatG)
            );
        }

        public async Task DeleteAsync(int id)
        {
            await Init();

            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM MealEntry WHERE Id = @id;";
            cmd.Parameters.AddWithValue("@id", id);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}