using System;
using System.Collections.Generic;
using System.Text;

namespace StatsTrackerV2.Data
{
    public class DrawableStatistic
    {
        public PointF Location { get; set; }

        public Color Color { get; set; }

        public bool IsFirstHalf {  get; set; }

        public DrawableStatistic(PointF location, Color color, bool isFirstHalf)
        {
            Location = location;
            Color = color;
            IsFirstHalf = isFirstHalf;
        }
    }
}
