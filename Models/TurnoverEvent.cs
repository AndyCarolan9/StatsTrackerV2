namespace StatsTrackerV2.Models
{
    public class TurnoverEvent : MatchEvent
    {
        #region Constructors
        public TurnoverEvent() : base()
        {

        }

        public TurnoverEvent(PointF location, string? player, long time, EventType eventType, Team? team, int halfIndex,
            TurnoverType turnoverType) : base(location, player, time, eventType, team, halfIndex)
        {
            TurnoverType = turnoverType;
        }
        #endregion

        #region Properties
        public TurnoverType TurnoverType { get; set; }
        #endregion

        public override string ToString()
        {
            string formattedTime = FormatTime();
            string eventTypeString = Type.GetEventName();
            string resultTypeString = TurnoverType.ToString();

            return formattedTime + " " + TeamName + " " + eventTypeString + " via " + resultTypeString + " by " + Player;
        }
    }
}