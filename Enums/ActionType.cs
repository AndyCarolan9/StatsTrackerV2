namespace StatsTrackerV2.Enums
{
    /// <summary>
    /// Enum used to describe the action type of the match event.
    /// </summary>
    public enum ActionType
    {
        Default,
        Play,
        Free,
        Penalty,
        From45,
        Mark,
        Sideline,
    }

    static class ActionTypeExtensions
    {
        public static bool IsPlacedBallAction(this ActionType actionType)
        {
            return actionType != ActionType.Play;
        }
    }
}