namespace StatsTrackerV2.Models.EventScores
{
    public partial class TurnoverEventScore : EventScore
    {
        public TurnoverType TurnoverType { get; set; }

        public TurnoverEventScore(EventType eventType, int firstHalfGoals, int firstHalfPoints, int secondHalfGoals, int secondHalfPoints, TurnoverType turnoverType)
            : base(eventType, firstHalfGoals, firstHalfPoints, secondHalfGoals, secondHalfPoints)
        {
            TurnoverType = turnoverType;
            Title = turnoverType.GetDisplayString();
        }

        public TurnoverEventScore(EventType eventType, TurnoverType turnoverType) : base(eventType, 0, 0, 0, 0)
        {
            TurnoverType = turnoverType;
            Title = turnoverType.GetDisplayString();
        }

        public TurnoverEventScore(TurnoverType turnoverType) : base(EventType.TurnoverWon, 0, 0, 0, 0)
        {
            TurnoverType = turnoverType; 
            Title = turnoverType.GetDisplayString();
        }
    }
}
