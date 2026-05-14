using CommunityToolkit.Mvvm.ComponentModel;

namespace StatsTrackerV2.Models
{
    public enum KickoutDistance
    {
        Short,
        Medium,
        Long
    }

    public partial class KickoutDistanceData : ObservableObject
    {
        public KickoutDistance distance;

        [ObservableProperty]
        private string _kickoutDistance = string.Empty;

        [ObservableProperty]
        private int _totalWon = 0;

        [ObservableProperty]
        private int _totalKickouts = 0;

        [ObservableProperty]
        private double _percentage = 0;

        public KickoutDistanceData(KickoutDistance kickoutDistance)
        {
            distance = kickoutDistance;
            KickoutDistance = distance.ToString();
        }

        public void Reset()
        {
            TotalWon = 0;
            TotalKickouts = 0;
            Percentage = 0;
        }

        public void CalculatePercent()
        {
            if (TotalKickouts <= 0)
            {
                Percentage = 0;
            }

            double newPercent = (double)TotalWon / (double)TotalKickouts;
            newPercent *= 100;
            Percentage = Math.Round(newPercent, 2);
        }
    }
}
