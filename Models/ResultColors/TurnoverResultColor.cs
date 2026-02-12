using System;
using System.Collections.Generic;
using System.Text;

namespace StatsTrackerV2.Models.ResultColors
{
    public class TurnoverResultColor : EventResultColor
    {
        public TurnoverType TurnoverType { get; set; }

        public TurnoverResultColor(EventType type, TurnoverType turnoverType, Color color) : base(type, color)
        {
            TurnoverType = turnoverType;
            
            if(TurnoverType != TurnoverType.Default)
            {
                Name = TurnoverType.ToString();
            }
        }

        public TurnoverResultColor(EventType type, Color color) : base(type, color)
        {
            TurnoverType = TurnoverType.Default;
        }
    }
}
