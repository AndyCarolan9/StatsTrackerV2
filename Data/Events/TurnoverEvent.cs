using StatsTrackerV2.Models;

namespace StatsTrackerV2.Data.Events
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
    }
}