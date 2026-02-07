namespace StatsTrackerV2.Models.EventScores
{
    public partial class KickoutEventScore : EventScore
    {
        public KickOutResultType ResultType { get; set; }

        public KickoutEventScore(EventType eventType, int firstHalfGoals, int firstHalfPoints, int secondHalfGoals, int secondHalfPoints, KickOutResultType kickOutResultType)
            : base(eventType, firstHalfGoals, firstHalfPoints, secondHalfGoals, secondHalfPoints)
        {
            ResultType = kickOutResultType;
            Title = kickOutResultType.GetEventName();
        }

        public KickoutEventScore(KickOutResultType kickOutResultType) : base(EventType.KickOut, 0, 0, 0, 0)
        {
            ResultType = kickOutResultType;
            Title = kickOutResultType.GetEventName();
        }
    }
}
