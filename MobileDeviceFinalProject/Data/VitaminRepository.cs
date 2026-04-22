using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using MobileDeviceFinalProject.Models;

namespace MobileDeviceFinalProject.Data
{
    public class VitaminRepository
    {
        private bool _hasBeenInitialized;
        private readonly ILogger<VitaminRepository> _logger;

        public VitaminRepository(ILogger<VitaminRepository> logger)
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
                    CREATE TABLE IF NOT EXISTS VitaminEntry (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Dosage TEXT NOT NULL,
                        Benefits TEXT NOT NULL,
                        IsDaily INTEGER NOT NULL DEFAULT 1
                    );
                    CREATE TABLE IF NOT EXISTS VitaminLog (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        VitaminEntryId INTEGER NOT NULL,
                        LoggedDate TEXT NOT NULL
                    );";
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating Vitamin tables");
                throw;
            }

            _hasBeenInitialized = true;
        }

        public async Task<List<VitaminEntry>> GetAllAsync()
        {
            await Init();

            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM VitaminEntry ORDER BY Name";

            var vitamins = new List<VitaminEntry>();

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                vitamins.Add(new VitaminEntry
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Dosage = reader.GetString(2),
                    Benefits = reader.GetString(3),
                    IsDaily = reader.GetInt32(4) == 1
                });
            }

            return vitamins;
        }

        public async Task SaveVitaminAsync(VitaminEntry entry)
        {
            await Init();

            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var cmd = connection.CreateCommand();

            if (entry.Id == 0)
            {
                cmd.CommandText = @"
                    INSERT INTO VitaminEntry (Name, Dosage, Benefits, IsDaily)
                    VALUES (@Name, @Dosage, @Benefits, @IsDaily)";
            }
            else
            {
                cmd.CommandText = @"
                    UPDATE VitaminEntry
                    SET Name=@Name, Dosage=@Dosage, Benefits=@Benefits, IsDaily=@IsDaily
                    WHERE Id=@Id";
                cmd.Parameters.AddWithValue("@Id", entry.Id);
            }

            cmd.Parameters.AddWithValue("@Name", entry.Name);
            cmd.Parameters.AddWithValue("@Dosage", entry.Dosage);
            cmd.Parameters.AddWithValue("@Benefits", entry.Benefits);
            cmd.Parameters.AddWithValue("@IsDaily", entry.IsDaily ? 1 : 0);

            var rows = await cmd.ExecuteNonQueryAsync();
            _logger.LogInformation("Saved vitamin entry. Rows affected: {Rows}", rows);
        }

        public async Task DeleteVitaminAsync(int id)
        {
            await Init();

            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM VitaminEntry WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            await cmd.ExecuteNonQueryAsync();

            var cmd2 = connection.CreateCommand();
            cmd2.CommandText = "DELETE FROM VitaminLog WHERE VitaminEntryId = @id";
            cmd2.Parameters.AddWithValue("@id", id);
            await cmd2.ExecuteNonQueryAsync();
        }

        public async Task<List<VitaminLog>> GetLogsForDateAsync(DateTime date)
        {
            await Init();

            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM VitaminLog WHERE date(LoggedDate) = date(@date)";
            cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));

            var logs = new List<VitaminLog>();

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                logs.Add(new VitaminLog
                {
                    Id = reader.GetInt32(0),
                    VitaminEntryId = reader.GetInt32(1),
                    LoggedDate = DateTime.Parse(reader.GetString(2))
                });
            }

            return logs;
        }

        public async Task LogVitaminAsync(int vitaminId, DateTime date)
        {
            await Init();

            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = @"
                SELECT COUNT(*) FROM VitaminLog
                WHERE VitaminEntryId = @id AND date(LoggedDate) = date(@date)";
            checkCmd.Parameters.AddWithValue("@id", vitaminId);
            checkCmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));

            var count = (long)(await checkCmd.ExecuteScalarAsync())!;
            if (count > 0) return;

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO VitaminLog (VitaminEntryId, LoggedDate)
                VALUES (@id, @date)";
            cmd.Parameters.AddWithValue("@id", vitaminId);
            cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd HH:mm:ss"));

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UnlogVitaminAsync(int vitaminId, DateTime date)
        {
            await Init();

            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                DELETE FROM VitaminLog
                WHERE VitaminEntryId = @id AND date(LoggedDate) = date(@date)";
            cmd.Parameters.AddWithValue("@id", vitaminId);
            cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<int> GetStreakAsync(int vitaminId)
        {
            await Init();

            await using var connection = new SqliteConnection(Constants.DatabasePath);
            await connection.OpenAsync();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                SELECT date(LoggedDate)
                FROM VitaminLog
                WHERE VitaminEntryId = @id
                ORDER BY LoggedDate DESC";
            cmd.Parameters.AddWithValue("@id", vitaminId);

            var dates = new List<DateTime>();

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                dates.Add(DateTime.Parse(reader.GetString(0)));
            }

            if (dates.Count == 0) return 0;

            var check = DateTime.Today;

            if (dates[0].Date != check && dates[0].Date != check.AddDays(-1))
                return 0;

            if (dates[0].Date != check)
                check = check.AddDays(-1);

            int streak = 0;

            foreach (var d in dates)
            {
                if (d.Date == check)
                {
                    streak++;
                    check = check.AddDays(-1);
                }
                else break;
            }

            return streak;
        }
    }
}
