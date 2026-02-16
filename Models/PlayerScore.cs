using CommunityToolkit.Mvvm.ComponentModel;

namespace StatsTrackerV2.Models
{
    public partial class PlayerScore : ObservableObject
    {
        public int Goals {  get; set; }

        public int Points { get; set; }

        public int ScoredShots { get; set; }

        public int TotalShots {  get; set; }

        [ObservableProperty]
        private string _playerName = string.Empty;

        [ObservableProperty]
        private string _score = string.Empty;

        [ObservableProperty]
        private string _shootingPercentage = string.Empty;

        public PlayerScore(string playerName)
        {
            PlayerName = playerName;
        }

        public void CalculateData()
        {
            if(TotalShots <= 0)
                return;

            Score = Goals + "-" + Points;
            float percentage = (float)ScoredShots / (float)TotalShots;

            ShootingPercentage = $"{ScoredShots} / {TotalShots} ({percentage:N2})";
        }
    }
}
