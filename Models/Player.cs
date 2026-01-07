using CommunityToolkit.Mvvm.ComponentModel;

namespace StatsTrackerV2.Models
{
    public partial class Player : ObservableObject
    {
        [ObservableProperty]
        public string _index;

        public string Name { get; set; }
    }
}
