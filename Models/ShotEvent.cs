namespace StatsTrackerV2.Models
{
    public class ShotEvent : MatchEvent
    {
        #region Constructors
        public ShotEvent() : base()
        {

        }

        public ShotEvent(PointF location, string? player, long time, EventType eventType, Team? team, int halfIndex,
            ActionType actionType, ShotResultType resultType) : base(location, player, time, eventType, team, halfIndex)
        {
            ActionType = actionType;
            ResultType = resultType;
        }
        #endregion

        #region Properties
        public ActionType ActionType { get; set; }

        public ShotResultType ResultType { get; set; }
        #endregion

        public override string ToString()
        {
            string formattedTime = FormatTime();
            string eventTypeString = Type.GetEventName();
            string resultTypeString = ResultType.GetEventName();

            return formattedTime + " " + TeamName + " " + eventTypeString + "-" + resultTypeString + " from "
                   + ActionType + " by " + Player;
        }
    }
}