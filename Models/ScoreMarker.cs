using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace StatsTrackerV2.Models
{
    public partial class ScoreMarker : ObservableObject
    {
        [ObservableProperty]
        public DateTime _time;

        [ObservableProperty]
        public int _score;

        public ScoreMarker(DateTime time, int score)
        {
            _time = time;
            _score = score;
        }
    }
}
