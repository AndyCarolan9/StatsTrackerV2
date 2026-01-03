using System.Text.Json.Serialization;

namespace StatsTrackerV2.Data.Events
{
    /// <summary>
    /// Match Event class which is used to define when a relevant statistic is recorded.
    /// </summary>
    [JsonDerivedType(typeof(MatchEvent), typeDiscriminator: "MatchEvent")]
    [JsonDerivedType(typeof(KickOutEvent), typeDiscriminator: "KickOutEvent")]
    [JsonDerivedType(typeof(ShotEvent), typeDiscriminator: "ShotEvent")]
    [JsonDerivedType(typeof(SubstitutionEvent), typeDiscriminator: "SubstitutionEvent")]
    [JsonDerivedType(typeof(TurnoverEvent), typeDiscriminator: "TurnoverEvent")]
    public class MatchEvent
    {
        #region Constructors
        public MatchEvent()
        {
            Player = string.Empty;
            TeamName = string.Empty;
        }

        public MatchEvent(PointF location, string? player, long time, EventType eventType, Team? team, int halfIndex)
        {
            Location = location;
            Player = player is null ? string.Empty : player;
            Time = time;
            Type = eventType;
            TeamName = team is null ? string.Empty : team.TeamName;
            HalfIndex = halfIndex;
        }
        #endregion

        #region Properties

        public PointF Location { get; set; }

        public string Player { get; set; }

        public long Time { get; set; }

        public EventType Type { get; set; }

        public string TeamName { get; set; }

        public int HalfIndex { get; set; }

        #endregion

        public override string ToString()
        {
            string formattedTime = FormatTime();
            string eventTypeString = Type.GetEventName();
            return formattedTime + " " + TeamName + " " + eventTypeString + " " + Player;
        }

        protected string FormatTime()
        {
            TimeSpan time = TimeSpan.FromMilliseconds(Time);
            string half = HalfIndex == 1 ? "1st" : "2nd";
            string seconds = time.Seconds < 10 ? "0" + time.Seconds.ToString() : time.Seconds.ToString();
            return time.Minutes + ":" + seconds + " mins " + half + " half";
        }
    }
}

