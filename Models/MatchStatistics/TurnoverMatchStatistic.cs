namespace StatsTrackerV2.Models.MatchStatistics
{
    public partial class TurnoverMatchStatistic : MatchStatistic
    {
        public TurnoverType TurnoverType { get; set; }

        public TurnoverMatchStatistic(EventType type, string name, int firstHalfValue, int secondHalfValue)
            : base(type, name, firstHalfValue, secondHalfValue)
        { 
            TurnoverType = TurnoverType.Default;
        }

        public TurnoverMatchStatistic(EventType type, TurnoverType turnoverType, string name, int firstHalfValue, int secondHalfValue)
            : base(type, name, firstHalfValue, secondHalfValue)
        {
            TurnoverType = turnoverType;
        }
    }
}
