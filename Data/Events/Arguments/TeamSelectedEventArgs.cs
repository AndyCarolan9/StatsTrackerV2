namespace StatsTrackerV2.Data.Events.Arguments
{
    public class TeamSelectedEventArgs : EventArgs
    {
        public string? HomeTeamName { get; set; }

        public string? AwayTeamName { get; set; }

        public string[]? HomePlayers { get; set; }

        public string[]? AwayPlayers { get; set; }

        public Color? HomeTeamColor { get; set; }

        public Color? AwayTeamColor { get; set; }
    }
}