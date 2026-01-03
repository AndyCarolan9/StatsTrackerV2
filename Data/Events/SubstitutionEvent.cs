namespace StatsTrackerV2.Data.Events
{
    public class SubstitutionEvent : MatchEvent
    {
        #region Constructors
        public SubstitutionEvent()
        {
            PlayerOnName = string.Empty;
        }

        public SubstitutionEvent(string playerOffName, string playerOnName, long time, EventType eventType, Team? team,
            int halfIndex) : base(new PointF(), playerOffName, time, eventType, team, halfIndex)
        {
            PlayerOnName = playerOnName;
        }
        #endregion

        #region Properties
        public string PlayerOnName { get; set; }
        #endregion

        public override string ToString()
        {
            string formattedTime = FormatTime();
            string eventTypeString = Type.GetEventName();

            return formattedTime + " " + TeamName + " " + eventTypeString + "-On: "
                + PlayerOnName + "- Off: " + Player;
        }
    }
}