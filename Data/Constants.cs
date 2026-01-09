namespace StatsTrackerV2.Data
{
    public static class Constants
    {
        public const string DatabaseFilename = "AppSQLite.db3";

        public const string TeamsJSONFilename = "Teams.json";

        public const string TeamsFolder = "Teams";

        public const string MatchesFolder = "Matches";

        public static string DatabasePath =>
            $"Data Source={Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename)}";

        public static string TeamsJSONPath =>
            $"{Path.Combine(FileSystem.AppDataDirectory, TeamsFolder, TeamsJSONFilename)}";

        public static string MatchesFolderPath =>
            $"{Path.Combine(FileSystem.AppDataDirectory, MatchesFolder)}";
    }
}