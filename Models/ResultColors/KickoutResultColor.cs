namespace StatsTrackerV2.Models.ResultColors
{
    public class KickoutResultColor : EventResultColor
    {
        public KickOutResultType ResultType { get; set; }

        public KickoutResultColor(KickOutResultType type, Color color)
            : base(EventType.KickOut, color)
        {
            ResultType = type;
            Name = ResultType.GetEventName();
        }
    }
}
