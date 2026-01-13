using StatsTrackerV2.Models;

namespace StatsTrackerV2.Data.Arguments
{
    public class InputStatEventArgs
    {
        public EventType EventType { get; set; }

        public Team? Team { get; set; }

        public PointF Location { get; set; }

        public string? Player { get; set; }
    }
}

