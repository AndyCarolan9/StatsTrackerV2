using System;
using System.Collections.Generic;
using System.Text;

namespace StatsTrackerV2.Data
{
    public class DrawableStatistic
    {
        public int Index { get; set; }

        public PointF Location { get; set; }

        public Color Color { get; set; }

        public bool IsFirstHalf {  get; set; }

        public DrawableStatistic(int index, PointF location, Color color, bool isFirstHalf)
        {
            Index = index;
            Location = location;
            Color = color;
            IsFirstHalf = isFirstHalf;
        }
    }
}
