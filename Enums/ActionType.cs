namespace StatsTrackerV2.Enums
{
    /// <summary>
    /// Enum used to describe the action type of the match event.
    /// </summary>
    public enum ActionType
    {
        Default,
        Sideline,
        Penalty,
        Free,
        From45,
        Play,
        Mark,
    }

    static class ActionTypeExtensions
    {
        public static bool IsPlacedBallAction(this ActionType actionType)
        {
            return actionType != ActionType.Play;
        }
    }
}