namespace StatsTrackerV2.Data
{
    public static class Constants
    {
        public const string DatabaseFilename = "AppSQLite.db3";

        public const string TeamsJSONFilename = "Teams.json";

        public static string DatabasePath =>
            $"Data Source={Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename)}";

        public static string TeamsJSONPath =>
            $"{Path.Combine(FileSystem.AppDataDirectory, "Teams", TeamsJSONFilename)}";
    }
}