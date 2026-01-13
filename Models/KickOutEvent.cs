namespace StatsTrackerV2.Models
{
    public class KickOutEvent : MatchEvent
    {
        #region Constructors
        public KickOutEvent() : base()
        {

        }

        public KickOutEvent(PointF location, string? player, long time, EventType eventType, Team? team, int halfIndex,
            KickOutResultType resultType) : base(location, player, time, eventType, team, halfIndex)
        {
            ResultType = resultType;
        }
        #endregion

        #region Properties
        public KickOutResultType ResultType { get; set; }
        #endregion

        public override string ToString()
        {
            string formattedTime = FormatTime();
            string eventTypeString = Type.GetEventName();
            string resultTypeString = ResultType.GetEventName();

            return formattedTime + " " + TeamName + " " + eventTypeString + "-" + resultTypeString + " by " + Player;
        }
    }
}

