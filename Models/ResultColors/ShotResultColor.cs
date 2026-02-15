namespace StatsTrackerV2.Models.ResultColors
{
    public class ShotResultColor : EventResultColor
    {
        public ShotResultType ResultType { get; set; }

        public ShotResultColor(EventType type, ShotResultType resultType, Color color)
            : base(type, color)
        {
            ResultType = resultType;
            Name = ResultType.GetEventName();

            if(ResultType == ShotResultType.DoublePoint)
            {
                Name = "2 Pointer";
            }
        }
    }
}
