using System;
using System.Collections.Generic;
using System.Text;

namespace StatsTrackerV2.Utilities
{
    public static class ColorsHelper
    {
        public static bool IsColorDark(Color color)
        {
            double rBrightness = color.Red * 0.299;
            double gBrightness = color.Green * 0.587;
            double bBrighness = color.Blue * 0.114;

            double brightness = rBrightness + gBrightness + bBrighness;
            return brightness < 0.5;
        }
    }
}
