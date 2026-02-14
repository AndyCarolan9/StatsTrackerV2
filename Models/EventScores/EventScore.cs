using CommunityToolkit.Mvvm.ComponentModel;

namespace StatsTrackerV2.Models.EventScores
{
    public partial class EventScore : ObservableObject
    {
        public EventType EventType {  get; set; }

        public int FirstHalfGoals { get; set; }

        public int FirstHalfPoints { get; set; }

        public int SecondHalfGoals { get; set; }

        public int SecondHalfPoints { get; set; }

        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private string _firstHalfValue = string.Empty;

        [ObservableProperty]
        private string _secondHalfValue = string.Empty;

        public EventScore(EventType eventType, int firstHalfGoals, int firstHalfPoints, int secondHalfGoals, int secondHalfPoints)
        {
            EventType = eventType;
            FirstHalfGoals = firstHalfGoals;
            FirstHalfPoints = firstHalfPoints;
            SecondHalfGoals = secondHalfGoals;
            SecondHalfPoints = secondHalfPoints;
            Title = eventType.GetEventName();
            UpdateScoreValues();
        }

        public string GetFirstHalfString()
        {
            return FirstHalfGoals + "-" + FirstHalfPoints;
        }

        public string GetSecondHalfString()
        {
            return SecondHalfGoals + "-" + SecondHalfPoints;
        }

        public void UpdateScoreValues(bool shouldReset = false)
        {
            if (shouldReset)
            {
                FirstHalfGoals = 0;
                FirstHalfPoints = 0;
                SecondHalfGoals = 0;
                SecondHalfPoints = 0;
            }

            FirstHalfValue = GetFirstHalfString();
            SecondHalfValue = GetSecondHalfString();
        }
    }
}
