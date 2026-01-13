namespace StatsTrackerV2.Data.Arguments
{
    public class ShotEventArgs : InputStatEventArgs
    {
        public ActionType ActionType { get; set; }

        public ShotResultType ResultType { get; set; }

        public bool IsTurnedOver
        {
            get
            {
                if (ResultType.IsPossessionTurnedOver())
                {
                    return true;
                }

                return _isTurnedOver;
            }
            set => _isTurnedOver = value;
        }

        private bool _isTurnedOver = false;

        public bool IsPossessionButtonsVisible()
        {
            if (!ResultType.IsPossessionTurnedOver())
            {
                if (ResultType.IsOutFor45())
                {
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}