using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace StatsTrackerV2.Models
{
    public partial class ScoreMarker : ObservableObject
    {
        [ObservableProperty]
        public double _elapsedSeconds;

        [ObservableProperty]
        public int _score;

        public ScoreMarker(double elapsedSeconds, int score)
        {
            ElapsedSeconds = elapsedSeconds;
            Score = score;
        }
    }
}
