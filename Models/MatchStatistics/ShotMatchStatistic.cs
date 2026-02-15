namespace StatsTrackerV2.Models.MatchStatistics
{
    public partial class ShotMatchStatistic : MatchStatistic
    {
        public ShotResultType ResultType { get; set; }

        public ShotMatchStatistic(EventType type, ShotResultType resultType, string name, int firstHalfValue, int secondHalfValue)
            : base(type, name, firstHalfValue, secondHalfValue)
        {
            ResultType = resultType;
        }

        public ShotMatchStatistic(ShotResultType resultType, string name, int firstHalfValue, int secondHalfValue)
            : base(EventType.Shots, name, firstHalfValue, secondHalfValue)
        {
            ResultType = resultType;
        }

        public ShotMatchStatistic(EventType type, string name, int firstHalfValue, int secondHalfValue)
            : base(type, name, firstHalfValue, secondHalfValue)
        {
            ResultType = ShotResultType.Default;
        }
    }
}
