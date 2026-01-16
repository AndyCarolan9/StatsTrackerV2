using CommunityToolkit.Mvvm.ComponentModel;

namespace StatsTrackerV2.Models
{
    public partial class MatchStatistic : ObservableObject
    {
        public EventType EventType { get; set; }

        [ObservableProperty]
        public string _name;

        [ObservableProperty]
        public int _firstHalfValue;

        [ObservableProperty]
        public int _secondHalfValue;

        public MatchStatistic(EventType type, string name, int firstHalfValue, int secondHalfValue)
        {
            EventType = type;
            Name = name;
            FirstHalfValue = firstHalfValue;
            SecondHalfValue = secondHalfValue;
        }
    }
}
