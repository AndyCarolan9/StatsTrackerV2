namespace StatsTrackerV2.Models.MatchStatistics
{
    public partial class KickoutMatchStatistic : MatchStatistic
    {
        public KickOutResultType KickOutResultType { get; set; }

        public KickoutMatchStatistic(KickOutResultType resultType, string name, int firstHalfValue, int secondHalfValue)
            : base(EventType.KickOut, name, firstHalfValue, secondHalfValue)
        {
            KickOutResultType = resultType;
        }
    }
}
