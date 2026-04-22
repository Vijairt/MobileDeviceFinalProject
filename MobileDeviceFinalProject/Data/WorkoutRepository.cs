using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using MobileDeviceFinalProject.Models;

namespace MobileDeviceFinalProject.Data
{
    public class WorkoutRepository
    {
        private bool _hasBeenInitialized;
        private readonly ILogger<WorkoutRepository> _logger;

        public WorkoutRepository(ILogger<WorkoutRepository> logger)
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
                    CREATE TABLE IF NOT EXISTS WorkoutEntry (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        ExerciseType TEXT NOT NULL,
                        DurationMinutes INTEGER NOT NULL,
                        DistanceMiles REAL,
                        Sets INTEGER,
                        Reps INTEGER,
                        Notes TEXT,
                        LoggedAt TEXT NOT NULL
                    );";
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating WorkoutEntry table");
                throw;
            }

            _hasBeenInitialized = true;
        }

        public async Task<List<WorkoutEntry>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            await Init();

            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM WorkoutEntry WHERE date(LoggedAt) BETWEEN date(@start) AND date(@end) ORDER BY LoggedAt DESC";
            cmd.Parameters.AddWithValue("@start", start.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@end", end.ToString("yyyy-MM-dd"));

            var workouts = new List<WorkoutEntry>();

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                workouts.Add(new WorkoutEntry
                {
                    Id = reader.GetInt32(0),
                    ExerciseType = reader.GetString(1),
                    DurationMinutes = reader.GetInt32(2),
                    DistanceMiles = reader.IsDBNull(3) ? null : reader.GetDouble(3),
                    Sets = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                    Reps = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                    Notes = reader.IsDBNull(6) ? null : reader.GetString(6),
                    LoggedAt = DateTime.Parse(reader.GetString(7))
                });
            }

            return workouts;
        }

        public async Task SaveAsync(WorkoutEntry entry)
        {
            await Init();

            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO WorkoutEntry (ExerciseType, DurationMinutes, DistanceMiles, Sets, Reps, Notes, LoggedAt)
                VALUES (@ExerciseType, @DurationMinutes, @DistanceMiles, @Sets, @Reps, @Notes, @LoggedAt)";

            cmd.Parameters.AddWithValue("@ExerciseType", entry.ExerciseType);
            cmd.Parameters.AddWithValue("@DurationMinutes", entry.DurationMinutes);
            cmd.Parameters.AddWithValue("@DistanceMiles", (object?)entry.DistanceMiles ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Sets", (object?)entry.Sets ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Reps", (object?)entry.Reps ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Notes", (object?)entry.Notes ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@LoggedAt", entry.LoggedAt.ToString("yyyy-MM-dd HH:mm:ss"));

            var rows = await cmd.ExecuteNonQueryAsync();
            _logger.LogInformation("Saved workout entry. Rows affected: {Rows}", rows);
        }

        public async Task DeleteAsync(int id)
        {
            await Init();

            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM WorkoutEntry WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
