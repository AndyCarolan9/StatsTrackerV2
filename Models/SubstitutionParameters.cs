using CommunityToolkit.Mvvm.ComponentModel;

namespace StatsTrackerV2.Models
{
    public partial class SubstitutionParameters : ObservableObject
    {
        [ObservableProperty]
        private string _playerOn = string.Empty;

        [ObservableProperty]
        private string _playerOff = string.Empty;

        public SubstitutionParameters()
        {

        }
    }
}
