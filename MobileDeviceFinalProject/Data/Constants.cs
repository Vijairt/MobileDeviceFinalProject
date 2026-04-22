namespace MobileDeviceFinalProject
{
    public static class Constants
    {
        public static string DatabasePath =>
            $"Data Source={Path.Combine(FileSystem.AppDataDirectory, "fittrack.db")}";
    }
}