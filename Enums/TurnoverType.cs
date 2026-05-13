namespace StatsTrackerV2.Enums
{
    public enum TurnoverType
    {
        Default,
        Intercept,
        Free,
        Tackle,
        BreakingBall
    }

    static class TurnoverTypeExtensions
    {
        public static string GetDisplayString(this TurnoverType type)
        {
            return string.Concat(type.ToString().Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
        }
    }
}